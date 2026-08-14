using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public AuthResponseDto() { }
        public AuthResponseDto(
            string accessToken,
            string refreshToken,
            DateTime expiresAt,
            int userId,
            string email,
            string role)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresAt = expiresAt;
            UserId = userId;
            Email = email;
            Role = role;
        }
    }
}
