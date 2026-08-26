using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace HospitalManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _hasher;
        public UserService(IApplicationDbContext context, IPasswordHasher hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        private static UserDto ToDto(User u) => new(
            u.UserId, u.Username, u.Email, u.Phone, u.Role?.RoleName ?? "Unknown", u.IsActive, u.CreatedAt);

        public async Task<UserDto> CreateAsync(CreateUserRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                throw new InvalidOperationException("Username already exists.");

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == request.Role);
            if (role is null)
                throw new ArgumentException($"Role '{request.Role}' does not exist. Valid roles: Admin, Doctor, Patient.");

            var user = new User
            {
                Username = request.Username,
                PasswordHash = _hasher.Hash(request.Password),
                Email = request.Email,
                Phone = request.Phone,
                RoleId = role.RoleId,
                IsActive = true,
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UserDto(user.UserId, user.Username, user.Email, user.Phone, role.RoleName, user.IsActive, user.CreatedAt);
        }

        public async Task<UserDto?> UpdateAsync(
            int id,
            UpdateUserRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user is null)
                return null;

            // Check duplicate username
            var usernameExists = await _context.Users
                .AnyAsync(u =>
                    u.UserId != id &&
                    u.Username == request.Username);

            if (usernameExists)
            {
                throw new InvalidOperationException(
                    "Username already exists.");
            }

            // Check duplicate email
            var emailExists = await _context.Users
                .AnyAsync(u =>
                    u.UserId != id &&
                    u.Email == request.Email);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "Email already exists.");
            }

            // Update user
            user.Username = request.Username;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.RoleId= (await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == request.Role))?.RoleId ?? user.RoleId;
            await _context.SaveChangesAsync();

            return new UserDto(
         user.UserId,
         user.Username,
         user.Email,
         user.Phone,
         user.Role?.RoleName ?? string.Empty,
         user.IsActive,
         user.CreatedAt
     );
        }

        public async Task<List<UserDto>> GetAllAsync() =>
            (await _context.Users.Include(u => u.Role).AsNoTracking().ToListAsync())
                .Select(ToDto).ToList();

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
            return user is null ? null : ToDto(user);
        }

        public async Task<bool> UpdateStatusAsync(int id, UpdateUserStatusRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return false;

            user.IsActive = request.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}