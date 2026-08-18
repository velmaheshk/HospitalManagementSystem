using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HospitalManagement.Application.DTO
{
    public class UpdateAppointmentDto
    {
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string TimeSlot { get; set; } = string.Empty;

        public string? Reason { get; set; }
    }
}
