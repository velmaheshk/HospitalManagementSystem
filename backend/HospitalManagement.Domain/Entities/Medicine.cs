using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>Pharmacy catalog and stock ledger.</summary>
    public class Medicine
    {
        public int MedicineId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Manufacturer { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int ReorderLevel { get; set; } = 10;

        // Navigation
        public ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();
    }
}
