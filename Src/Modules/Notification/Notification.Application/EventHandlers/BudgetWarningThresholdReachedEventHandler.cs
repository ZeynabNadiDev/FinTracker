using FinTracker.SharedKernel.EventsContracts;
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
    public sealed class BudgetWarningThresholdReachedEventHandler
     : INotificationHandler<BudgetWarningThresholdReachedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BudgetWarningThresholdReachedEventHandler(
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            BudgetWarningThresholdReachedEvent notification,
            CancellationToken cancellationToken)
        {
            var title = "Budget Warning";
            var message =
                $"You have spent {notification.Percentage:N0}% of your budget. " +
                $"Target: {notification.TargetAmount:N0}, spent: {notification.CurrentSpentAmount:N0}.";

            var notificationEntity = NotificationEntity.Create(
                Guid.NewGuid(),
                notification.UserId,
                title,
                message,
                NotificationType.BudgetWarning);

            await _notificationRepository.AddAsync(notificationEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
