using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
