using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>Extends a Doctor-role User with professional details.</summary>
    public class Doctor
    {
        public int DoctorId { get; set; }

        // FK -> Users.UserId (1:1, unique)
        public int UserId { get; set; }
        public User? User { get; set; }

        // FK -> Departments.DepartmentId
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string? Qualification { get; set; }
        public int? ExperienceYears { get; set; }
        public decimal ConsultationFee { get; set; }

        // Navigation
        public ICollection<DoctorAvailability> Availabilities { get; set; } = new List<DoctorAvailability>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
