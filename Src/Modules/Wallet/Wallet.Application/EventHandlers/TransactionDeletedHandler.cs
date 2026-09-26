using MediatR;
using Transaction.Domain.Entities.Transaction.Events;
using Transaction.Domain.Enums;
using Wallet.Domain.Repository;
using Wallet.Domain.UOW;

namespace Wallet.Application.EventHandlers
{
    public class TransactionDeletedEventHandler : INotificationHandler<TransactionDeleted>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionDeletedEventHandler(
            IWalletRepository walletRepository,
            IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            TransactionDeleted notification,
            CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(
                notification.WalletId,
                cancellationToken);

            if (wallet is null)
            {
                // In production, log a warning or handle gracefully.
                return;
            }

            // Reverse the balance change made when the transaction was created.
            if (notification.Type == TransactionType.Income)
            {
                wallet.Withdraw(notification.Amount);
            }
            else if (notification.Type == TransactionType.Expense)
            {
                wallet.Deposit(notification.Amount);
            }

            _walletRepository.Update(wallet);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
