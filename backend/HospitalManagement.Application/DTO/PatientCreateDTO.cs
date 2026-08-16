using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Application.DTO
{
    public class PatientCreateDTO
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateTime DOB { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? BloodGroup { get; set; }

        public string? EmergencyContactName { get; set; }

        public string? EmergencyContactPhone { get; set; }
    }
}
