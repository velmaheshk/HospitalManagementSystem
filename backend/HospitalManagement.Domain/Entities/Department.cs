using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>Hospital departments used to group doctors (supports the Doctor module).</summary>
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;

        // Navigation
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
