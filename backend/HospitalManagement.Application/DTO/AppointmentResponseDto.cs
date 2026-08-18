using System;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.DTOs.Appointment
{
    public class AppointmentResponseDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;

        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;

        public DateTime AppointmentDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; }

        public string? Reason { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}