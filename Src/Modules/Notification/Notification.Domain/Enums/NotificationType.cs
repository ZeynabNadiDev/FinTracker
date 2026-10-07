using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Enums
{
    public enum NotificationType
    {
        General = 1,
        BudgetWarning = 2,
        BudgetExceeded = 3,
        WalletTransfer = 4
    }
}
