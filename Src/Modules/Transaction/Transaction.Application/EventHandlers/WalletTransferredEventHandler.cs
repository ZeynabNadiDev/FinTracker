using FinTracker.SharedKernel.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Transaction.Domain.Entities.Transaction;
using Transaction.Domain.Repositories;
using Transaction.Domain.UOW;

namespace Transaction.Application.EventHandlers
{
    public sealed class WalletTransferredEventHandler : INotificationHandler<WalletTransferredEvent>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WalletTransferredEventHandler(
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Handle(WalletTransferredEvent notification, CancellationToken cancellationToken)
        {
            // Create transfer transaction aggregate using the factory method
            var transferTransaction = Transaction.Domain.Entities.Transaction.Transaction.CreateTransfer(
                userId: notification.UserId,
                sourceWalletId: notification.SourceWalletId,
                destinationWalletId: notification.DestinationWalletId,
                amount: notification.Amount,
                description: $"Transfer from wallet {notification.SourceWalletId} to {notification.DestinationWalletId}"
            );

            // Add to database
            await _transactionRepository.AddAsync(transferTransaction, cancellationToken);

            // Commit changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
