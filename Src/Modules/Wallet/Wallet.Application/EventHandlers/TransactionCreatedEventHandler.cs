using MediatR;
using Transaction.Domain.Entities.Transaction.Events;
using Transaction.Domain.Enums;
using Wallet.Domain.Repository;
using Wallet.Domain.UOW;

namespace Wallet.Application.EventHandlers
{
    public class TransactionCreatedEventHandler : INotificationHandler<TransactionCreated>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionCreatedEventHandler(
            IWalletRepository walletRepository,
            IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(TransactionCreated notification, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(notification.WalletId, cancellationToken);
            if (wallet is null)
            {
                // In production, log warning or handle gracefully
                return;
            }

            // Adjust balance based on transaction type
            if (notification.Type == TransactionType.Income)
            {
                wallet.Deposit(notification.Amount); // Or your wallet method for adding balance
            }
            else if (notification.Type == TransactionType.Expense)
            {
                wallet.Withdraw(notification.Amount); // Or your wallet method for deducting balance
            }

             _walletRepository.Update(wallet);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
