using FinTracker.SharedKernel.Events;
using MediatR;
using Notification.Domain.Enums;
using Notification.Domain.Repositories;
using Notification.Domain.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationEntity = Notification.Domain.Entities.Notification.Notification;

namespace Notification.Application.EventHandlers
{
    public sealed class TransactionDeletedEventHandler : INotificationHandler<TransactionDeletedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionDeletedEventHandler(
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(TransactionDeletedEvent notification, CancellationToken cancellationToken)
        {
            var title = "Transaction Deleted";
            var message = $"A transaction of amount {notification.Amount:N0} has been deleted.";

            // Using the Create factory method instead of the constructor
            var notificationEntity = NotificationEntity.Create(
                Guid.NewGuid(),
                notification.UserId,
                title,
                message,
                NotificationType.General // Correct enum usage
            );

            await _notificationRepository.AddAsync(notificationEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
