using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>Individual medicine line within a prescription (feeds Pharmacy dispensing).</summary>
    public class PrescriptionItem
    {
        public int PrescriptionItemId { get; set; }

        // FK -> Prescriptions.PrescriptionId
        public int PrescriptionId { get; set; }
        public Prescription? Prescription { get; set; }

        // FK -> Medicines.MedicineId
        public int MedicineId { get; set; }
        public Medicine? Medicine { get; set; }

        public string Dosage { get; set; } = string.Empty;    // e.g. "500mg"
        public string Frequency { get; set; } = string.Empty; // e.g. "Twice daily"
        public int DurationDays { get; set; }
        public int Quantity { get; set; }
    }
}
