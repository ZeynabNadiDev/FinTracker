using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Commands.MarkNotificationAsRead
{
    public sealed record MarkNotificationAsReadCommand(
      Guid NotificationId,
      Guid UserId
  ) : IRequest;
}
