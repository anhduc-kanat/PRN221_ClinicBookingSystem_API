using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_BusinessObject.Enums
{
    public enum TransactionStatus
    {
        [Description("Pending")]
        Pending = 2,
        [Description("Paid")]
        Paid = 1,
        [Description("Cancelled")]
        Cancelled = 0,
        [Description("Overdue")]
        Overdue = 3
    }
}