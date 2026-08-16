using HospitalManagement.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HospitalManagement.Infrastructure.Security
{
    // public interface IJwtTokenGenerator
    //{
    //    string GenerateToken(User user);
    //}

    //public class JwtTokenGenerator : IJwtTokenGenerator
    //{
    //    private readonly IConfiguration _config;
    //    public JwtTokenGenerator(IConfiguration config) => _config = config;

    //    public string GenerateToken(User user)
    //    {
    //        var claims = new List<Claim>
    //        {
    //            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
    //            new(ClaimTypes.Name, user.Username),
    //            new(ClaimTypes.Email, user.Email),
    //            new(ClaimTypes.Role, user.Role?.RoleName ?? "Patient"),
    //        };

    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
    //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //        var token = new JwtSecurityToken(
    //            issuer: _config["Jwt:Issuer"],
    //            audience: _config["Jwt:Audience"],
    //            claims: claims,
    //            expires: DateTime.UtcNow.AddHours(8),
    //            signingCredentials: creds);

    //        return new JwtSecurityTokenHandler().WriteToken(token);
    //    }
    //}

    //public interface IPasswordHasher
    //{
    //    string Hash(string password);
    //    bool Verify(string password, string hash);
    //}

    //public class BCryptPasswordHasher : IPasswordHasher
    //{
    //    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    //    public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    //}
}
