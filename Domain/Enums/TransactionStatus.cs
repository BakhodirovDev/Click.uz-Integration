using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum TransactionStatus
    {
        [Description("Tranzaksiya muvaffaqiyatsiz tugagan")]
        Failed,
        [Description("To'lov qilish kutilmoqda")]
        Pending,
        [Description("Tranzaksiya muvaffaqiyatli tugagan")]
        Completed
    }
}
