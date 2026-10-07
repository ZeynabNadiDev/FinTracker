using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Commands.MarkAllNotificationsAsRead
{
    public sealed record MarkAllNotificationsAsReadCommand(Guid UserId) : IRequest;
}
