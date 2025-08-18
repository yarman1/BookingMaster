using BookingMaster.Dtos.Auth;

namespace BookingMaster.Interfaces
{
    public record Result<T>(bool Success, T? Data, string? Error);

    public interface IAuthService
    {
        Task<Result<AuthResponse>> RegisterOwnerAsync(RegisterRequest request);
        Task<Result<AuthResponse>> RegisterCustomerAsync(RegisterRequest request);
        Task<Result<AuthResponse>> LoginOwnerAsync(LoginRequest request);
        Task<Result<AuthResponse>> LoginCustomerAsync(LoginRequest request);
        Task<Result<bool>> LogoutAsync(string refreshToken);
        Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken);
    }
}
