using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    public enum AppointmentStatus { Scheduled, Completed, Cancelled, NoShow }

    /// <summary>Core booking record linking a Patient and a Doctor.</summary>
    public class Appointment
    {
        public int AppointmentId { get; set; }

        // FK -> Patients.PatientId
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        // FK -> Doctors.DoctorId
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty; // e.g. "10:30 AM - 10:45 AM"
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation — 1:1 optional
        public Prescription? Prescription { get; set; }
        public Bill? Bill { get; set; }
    }
}
