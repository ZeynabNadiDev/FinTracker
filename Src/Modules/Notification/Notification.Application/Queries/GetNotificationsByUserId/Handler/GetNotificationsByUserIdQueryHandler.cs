using MediatR;
using Notification.Application.Dtos;
using Notification.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Queries.GetNotificationsByUserId.Handler
{
    public class GetNotificationsByUserIdQueryHandler
     : IRequestHandler<GetNotificationsByUserIdQuery, List<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetNotificationsByUserIdQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<NotificationDto>> Handle(
            GetNotificationsByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetByUserIdAsync(
                request.UserId,
                request.OnlyUnread,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            return notifications
                .Select(n => new NotificationDto(
                    n.Id,
                    n.UserId,
                    n.Title,
                    n.Message,
                    (int)n.Type,
                    n.IsRead,
                    n.CreatedAt))
                .ToList();
        }
    }
}
