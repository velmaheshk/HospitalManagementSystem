using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>Extends a Patient-role User with demographic and medical-contact details.</summary>
    public class Patient
    {
        public int PatientId { get; set; }

        //FK -> Users.UserId (1:1, unique)
        public int UserId { get; set; }
        public User? User { get; set; }

        public string FullName { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? BloodGroup { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
}
