using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Dtos
{
    public record NotificationDto(
     Guid Id,
     Guid UserId,
     string Title,
     string Message,
     int Type,
     bool IsRead,
     DateTime CreatedAt);
}
