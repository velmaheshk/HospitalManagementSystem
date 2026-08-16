using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagement.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (
            string AccessToken,
            DateTime ExpiresAt)
            GenerateAccessToken(User user)
        {
            var key =
                _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "JWT Key is missing.");

            var issuer =
                _configuration["Jwt:Issuer"];

            var audience =
                _configuration["Jwt:Audience"];

            var expiresAt =
                DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<int>(
                        "Jwt:DurationInMinutes"));

            var claims =
                new List<Claim>
                {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.Username),

                new Claim(
                    ClaimTypes.Email,
                    user.Email),

                new Claim(
                    ClaimTypes.Role,
                    user.Role?.RoleName
                    ?? "Patient")
                };


            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key));

            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);


            var token =
                new JwtSecurityToken(
                    issuer: issuer,

                    audience: audience,

                    claims: claims,

                    expires: expiresAt,

                    signingCredentials: credentials);


            var accessToken =
                new JwtSecurityTokenHandler()
                    .WriteToken(token);

            return (
                accessToken,
                expiresAt);
        }


        public string GenerateRefreshToken()
        {
            var randomBytes =
                new byte[64];

            using var rng =
                RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Convert
                .ToBase64String(randomBytes);
        }
    }
}