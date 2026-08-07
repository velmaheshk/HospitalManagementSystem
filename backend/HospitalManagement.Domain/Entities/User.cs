using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>Central authentication table for every login (User Management module).</summary>
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }

        // FK -> Roles.RoleId
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation — 1:1 extensions depending on role
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
