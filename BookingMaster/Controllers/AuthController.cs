﻿using BookingMaster.Dtos.Auth;
using BookingMaster.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [AllowAnonymous]
        [HttpPost("register/owner")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterOwner([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterOwnerAsync(request);
            if (!result.Success) return BadRequest(result.Error);
            return Ok(result.Data);
        }

        [AllowAnonymous]
        [HttpPost("register/customer")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterCustomerAsync(request);
            if (!result.Success) return BadRequest(result.Error);
            return Ok(result.Data);
        }

        [AllowAnonymous]
        [HttpPost("login/owner")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginOwner([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginOwnerAsync(request);
            if (!result.Success) return Unauthorized(result.Error);
            return Ok(result.Data);
        }

        [AllowAnonymous]
        [HttpPost("login/customer")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginCustomer([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginCustomerAsync(request);
            if (!result.Success) return Unauthorized(result.Error);
            return Ok(result.Data);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.LogoutAsync(request.RefreshToken);
            if (!result.Success) return BadRequest(result.Error);
            return Ok(new { message = "Logged out successfully" });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            if (!result.Success) return Unauthorized(result.Error);
            return Ok(result.Data);
        }
    }
}
