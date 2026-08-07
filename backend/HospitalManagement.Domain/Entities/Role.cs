using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>Lookup table for the three login types: Admin, Doctor, Patient.</summary>
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;

        // Navigation
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
