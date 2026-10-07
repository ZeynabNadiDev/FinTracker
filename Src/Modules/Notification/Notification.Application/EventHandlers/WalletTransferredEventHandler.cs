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
    public sealed class WalletTransferredEventHandler : INotificationHandler<WalletTransferredEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WalletTransferredEventHandler(
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(WalletTransferredEvent notification, CancellationToken cancellationToken)
        {
            var title = "Wallet Transfer";
            var message = $"A wallet transfer of amount {notification.Amount:N0} was successfully completed.";

            var notificationEntity = NotificationEntity.Create(
                Guid.NewGuid(),
                notification.UserId,
                title,
                message,
                NotificationType.WalletTransfer
            );

            await _notificationRepository.AddAsync(notificationEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
