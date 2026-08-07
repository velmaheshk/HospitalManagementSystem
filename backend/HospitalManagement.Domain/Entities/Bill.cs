using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    public enum PaymentStatus { Pending, PartiallyPaid, Paid }

    /// <summary>One bill per appointment; rolls up consultation and pharmacy charges (Billing module).</summary>
    public class Bill
    {
        public int BillId { get; set; }

        // FK -> Patients.PatientId
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        // FK -> Appointments.AppointmentId (1:1, unique)
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        public DateTime BillDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public string? PaymentMode { get; set; } // Cash / Card / UPI / Insurance

        // Navigation
        public ICollection<BillItem> Items { get; set; } = new List<BillItem>();
    }
}
