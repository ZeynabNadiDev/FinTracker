using MediatR;
using Notification.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Queries.GetNotificationsByUserId
{
    public record GetNotificationsByUserIdQuery(
     Guid UserId,
     bool OnlyUnread,
     int PageNumber,
     int PageSize) : IRequest<List<NotificationDto>>;
}
