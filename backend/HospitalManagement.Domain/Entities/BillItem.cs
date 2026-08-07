using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>Individual charge line making up a Bill.</summary>
    public class BillItem
    {
        public int BillItemId { get; set; }

        // FK -> Bills.BillId
        public int BillId { get; set; }
        public Bill? Bill { get; set; }

        public string ItemType { get; set; } = string.Empty;    // Consultation / Medicine / Lab / Other
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
    }
}
