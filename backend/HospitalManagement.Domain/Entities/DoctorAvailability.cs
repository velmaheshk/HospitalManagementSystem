using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>Weekly recurring time window a doctor is bookable in (feeds Appointment slot search).</summary>
    public class DoctorAvailability
    {
        public int AvailabilityId { get; set; }

        // FK -> Doctors.DoctorId
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public string DayOfWeek { get; set; } = string.Empty; // Monday .. Sunday
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
