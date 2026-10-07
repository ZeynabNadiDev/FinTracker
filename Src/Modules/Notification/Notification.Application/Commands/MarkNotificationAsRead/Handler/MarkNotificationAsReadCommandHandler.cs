using FinTracker.SharedKernel.Exceptions;
using MediatR;
using Notification.Domain.Repositories;
using Notification.Domain.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Commands.MarkNotificationAsRead.Handler
{
    public sealed class MarkNotificationAsReadCommandHandler
     : IRequestHandler<MarkNotificationAsReadCommand>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkNotificationAsReadCommandHandler(
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            MarkNotificationAsReadCommand request,
            CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByIdAsync(
                request.NotificationId,
                cancellationToken);

            if (notification is null)
            {
                throw new DomainException("Notification was not found.");
            }

            if (notification.UserId != request.UserId)
            {
                throw new DomainException("You are not allowed to access this notification.");
            }

            notification.MarkAsRead();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
