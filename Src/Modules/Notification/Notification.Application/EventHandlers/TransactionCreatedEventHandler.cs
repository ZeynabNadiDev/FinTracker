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
    public class TransactionCreatedEventHandler : INotificationHandler<TransactionCreatedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionCreatedEventHandler(
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(TransactionCreatedEvent notification, CancellationToken cancellationToken)
        {
            var title = "Transaction Created";
            var message = $"A new transaction of amount {notification.Amount:N0} was successfully registered.";

            // Create domain entity using the factory method
            var notificationEntity = Notification.Domain.Entities.Notification.Notification.Create(
                Guid.NewGuid(),
                notification.UserId,
                title,
                message,
                NotificationType.General 
            );

            await _notificationRepository.AddAsync(notificationEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
