using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>One per appointment where the doctor prescribes medicine.</summary>
    public class Prescription
    {
        public int PrescriptionId { get; set; }

        // FK -> Appointments.AppointmentId (1:1, unique)
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        // FK -> Doctors.DoctorId
        public int DoctorId { get; set; }

        // FK -> Patients.PatientId
        public int PatientId { get; set; }

        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();
    }
}
