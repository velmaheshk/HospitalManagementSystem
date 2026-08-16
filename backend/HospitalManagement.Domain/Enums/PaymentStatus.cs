using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        PartiallyPaid = 3,
        Cancelled = 4
    }
}
