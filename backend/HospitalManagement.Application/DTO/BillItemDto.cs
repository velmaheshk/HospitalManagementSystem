using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Application.DTO
{
    internal class BillItemDto
    {
        public int BillId { get; set; }

        public int PatientId { get; set; }

        public int AppointmentId { get; set; }

        public DateTime BillDate { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public string? PaymentMode { get; set; }

        public ICollection<BillItemDto> Items { get; set; } = new List<BillItemDto>();
    }
}
