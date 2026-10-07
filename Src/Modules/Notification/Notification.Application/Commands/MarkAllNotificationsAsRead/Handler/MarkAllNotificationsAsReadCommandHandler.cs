using MediatR;
using Notification.Domain.Repositories;
using Notification.Domain.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Commands.MarkAllNotificationsAsRead.Handler
{
    public sealed class MarkAllNotificationsAsReadCommandHandler : IRequestHandler<MarkAllNotificationsAsReadCommand>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkAllNotificationsAsReadCommandHandler(
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var unreadNotifications = await _notificationRepository.GetUnreadByUserIdAsync(
                request.UserId,
                cancellationToken);

            if (unreadNotifications.Count == 0)
            {
                return;
            }

            foreach (var notification in unreadNotifications)
            {
                notification.MarkAsRead();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

}
