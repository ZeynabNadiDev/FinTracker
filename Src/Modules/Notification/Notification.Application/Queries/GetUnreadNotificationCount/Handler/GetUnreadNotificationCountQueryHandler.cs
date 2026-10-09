using MediatR;
using Notification.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Queries.GetUnreadNotificationCount.Handler
{
    public class GetUnreadNotificationCountQueryHandler
      : IRequestHandler<GetUnreadNotificationCountQuery, int>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetUnreadNotificationCountQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<int> Handle(
            GetUnreadNotificationCountQuery request,
            CancellationToken cancellationToken)
        {
            return await _notificationRepository.GetUnreadCountByUserIdAsync(
                request.UserId,
                cancellationToken);
        }
    }
}
