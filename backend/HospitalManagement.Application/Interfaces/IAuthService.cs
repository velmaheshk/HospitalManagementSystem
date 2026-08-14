using System;
using System.Collections.Generic;
using System.Text;
using HospitalManagement.Application.DTOs.Auth;

namespace HospitalManagement.Application.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user and returns JWT access token.
        /// </summary>
        Task<AuthResponseDto> LoginAsync(LoginDto request);

        /// <summary>
        /// Registers a new user.
        /// </summary>
        Task<AuthResponseDto> RegisterAsync(RegisterDto request);

        /// <summary>
        /// Generates a new access token using refresh token.
        /// </summary>
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);

        /// <summary>
        /// Revokes the refresh token.
        /// </summary>
        Task<bool> LogoutAsync(string refreshToken);

        /// <summary>
        /// Changes the current user's password.
        /// </summary>
        Task<bool> ChangePasswordAsync(
            int userId,
            ChangePasswordDto request);

        /// <summary>
        /// Checks whether the supplied email already exists.
        /// </summary>
        Task<bool> EmailExistsAsync(string email);
    }
}