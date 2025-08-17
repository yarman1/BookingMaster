using BookingMaster.Constants;
using BookingMaster.Data;
using BookingMaster.Dtos.Auth;
using BookingMaster.Helpers;
using BookingMaster.Interfaces;
using BookingMaster.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookingMaster.Services
{

    public class AuthService : IAuthService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDBContext context, IConfiguration config)
        {
            _dbContext = context;
            _config = config;
        }

        public async Task<Result<AuthResponse>> RegisterOwnerAsync(RegisterRequest request)
    => await RegisterAsync(request, Roles.Owner);

        public async Task<Result<AuthResponse>> RegisterCustomerAsync(RegisterRequest request)
            => await RegisterAsync(request, Roles.Customer);

        public async Task<Result<AuthResponse>> LoginOwnerAsync(LoginRequest request)
            => await LoginAsync(request, Roles.Owner);

        public async Task<Result<AuthResponse>> LoginCustomerAsync(LoginRequest request)
            => await LoginAsync(request, Roles.Customer);

        private async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, string roleName)
        {
            if (_dbContext.User.Any(user => user.Email == request.Email))
            {
                return new Result<AuthResponse>(false, null, "User with this email already exists");
            }

            var role = _dbContext.Role.FirstOrDefault(r => r.NormalizedName == roleName);
            if (role == null)
            {
                return new Result<AuthResponse>(false, null, "Role not found");
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = PasswordHelper.HashPassword(request.Password),
                Phone = request.Phone,
                RoleId = role.Id,
                Role = role
            };

            _dbContext.User.Add(user);
            await _dbContext.SaveChangesAsync();

            return new Result<AuthResponse>(true, await GenerateTokensAsync(user), null);
        }

        private async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, string role)
        {
            var user = await _dbContext.User.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == request.Email && u.Role.NormalizedName == role);
            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return new Result<AuthResponse>(false, null, "Invalid credentials");
            }

            if (user.RefreshToken != null)
            {
                _dbContext.RefreshToken.Remove(user.RefreshToken);
                await _dbContext.SaveChangesAsync();
            }

            return new Result<AuthResponse>(true, await GenerateTokensAsync(user), null);
        }

        public async Task<Result<bool>> LogoutAsync(string refreshToken)
        {
            var tokenEntity = await _dbContext.RefreshToken
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (tokenEntity == null || !tokenEntity.IsActive)
            {
                if (tokenEntity != null)
                {
                    _dbContext.RefreshToken.Remove(tokenEntity);
                    await _dbContext.SaveChangesAsync();
                }
                return new Result<bool>(false, false, "Invalid or expired refresh token");
            }

            _dbContext.RefreshToken.Remove(tokenEntity);
            await _dbContext.SaveChangesAsync();

            return new Result<bool>(true, true, null);
        }

        public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await _dbContext.RefreshToken
                .Include(rt => rt.User)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (tokenEntity == null || !tokenEntity.IsActive)
            {
                if (tokenEntity != null)
                {
                    _dbContext.RefreshToken.Remove(tokenEntity);
                    await _dbContext.SaveChangesAsync();
                }
                return new Result<AuthResponse>(false, null, "Invalid or expired refresh token");
            }
            
            _dbContext.RefreshToken.Remove(tokenEntity);
            await _dbContext.SaveChangesAsync();

            var newTokens = await GenerateTokensAsync(tokenEntity.User);
            return new Result<AuthResponse>(true, newTokens, null);
        }

        private async Task<AuthResponse> GenerateTokensAsync(User user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Email),
                new(ClaimTypes.Role, user.Role.NormalizedName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[18];
            rng.GetBytes(bytes);

            var refreshToken = Convert.ToBase64String(bytes)[..24];

            RefreshToken refreshTokenEntity = new()
            {
                UserId = user.Id,
                Token = refreshToken,
                Expiration = DateTime.UtcNow.AddDays(7),
                User = user,
            };

            _dbContext.RefreshToken.Add(refreshTokenEntity);
            await _dbContext.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
