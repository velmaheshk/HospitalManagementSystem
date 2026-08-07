using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>System-wide activity log feeding the Reports module and supporting traceability.</summary>
    public class AuditLog
    {
        public long LogId { get; set; }

        // FK -> Users.UserId
        public int UserId { get; set; }

        public string Module { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // e.g. "Created Appointment"
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Details { get; set; }
    }
}
