using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.DTOs.Auth;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HospitalManagement.Application.Services
{
    public class AuthService : IAuthService
    {
         private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenGenerator _jwt;

        public AuthService(IApplicationDbContext context, IPasswordHasher hasher, IJwtTokenGenerator jwt)
        {
          _context = context; _hasher = hasher; _jwt = jwt;
        }


        // ---------------------------------------------------------------
        // Shared helper: issue a fresh access + refresh token pair for a user
        // ---------------------------------------------------------------
        private async Task<AuthResponseDto> IssueTokensAsync(User user)
        {
            try
            {
                var (accessToken, expiresAt) = _jwt.GenerateAccessToken(user);
                var refreshToken = new RefreshToken
                {
                    UserId = user.UserId,
                    Token = _jwt.GenerateRefreshToken(),
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                };

                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();

                return new AuthResponseDto(
                    accessToken, refreshToken.Token, expiresAt,
                    user.UserId, user.Username, user.Role?.RoleName ?? "Patient");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == dto.Email);

            if (user is null || !user.IsActive || !_hasher.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            return await IssueTokensAsync(user);
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Email))
                throw new InvalidOperationException("Username already exists.");
            if (await EmailExistsAsync(dto.Email))
                throw new InvalidOperationException("Email is already registered.");

            // ✅ FIX: look up the role by the RoleId the client sent, not a hardcoded "Patient"
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleId == dto.RoleId);

            if (role == null)
                throw new InvalidOperationException($"Invalid RoleId: {dto.RoleId}");

            var user = new User
            {
                Username = dto.Email,
                PasswordHash = _hasher.Hash(dto.Password),
                Email = dto.Email,
                // Phone = dto.Phone,
                RoleId = role.RoleId,
                IsActive = true,
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // ✅ FIX: create the correct child record based on the role
            if (role.RoleName == "Patient")
            {
                var patient = new Patient
                {
                    UserId = user.UserId,
                    FullName = dto.FirstName + " " + dto.LastName,
                    DOB = DateTime.UtcNow.AddYears(-25), // placeholder — completed later via profile edit
                    Gender = "Unspecified",
                };
                await _context.Patients.AddAsync(patient);
            }
            else if (role.RoleName == "Doctor")
            {
                var doctor = new Doctor
                {
                    UserId = user.UserId,
                    FullName = dto.FirstName + " " + dto.LastName,
                    DepartmentId = 1, // ⚠️ placeholder — replace with real value or make selectable at registration
                };
                await _context.Doctors.AddAsync(doctor);
            }
            // Admin: no child profile table needed

            await _context.SaveChangesAsync();

            user.Role = role;
            return await IssueTokensAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            var existing = await _context.RefreshTokens.Include(rt => rt.User).ThenInclude(u => u!.Role)
                .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken);

            if (existing is null || !existing.IsActive)
                throw new UnauthorizedAccessException("Refresh token is invalid or has expired.");

            // Rotate: revoke the old token, issue a brand new pair
            existing.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return await IssueTokensAsync(existing.User!);
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            var existing = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if (existing is null || !existing.IsActive) return false; // already gone/expired — logout is idempotent

            existing.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null) return false;

            if (!_hasher.Verify(dto.CurrentPassword, user.PasswordHash))
                return false;

            user.PasswordHash = _hasher.Hash(dto.NewPassword);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
