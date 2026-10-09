using MediatR;
using FinTracker.SharedKernel.Events;
using Wallet.Domain.Repository;
using Wallet.Domain.UOW;
using FinTracker.SharedKernel.Enums;


namespace Wallet.Application.EventHandlers
{
    public class TransactionCreatedEventHandler : INotificationHandler<TransactionCreatedEvent>
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

        public async Task Handle(TransactionCreatedEvent notification, CancellationToken cancellationToken)
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
                wallet.Deposit(notification.Amount);
            }
            else if (notification.Type == TransactionType.Expense)
            {
                wallet.Withdraw(notification.Amount);
            }
            else if (notification.Type == TransactionType.Transfer && notification.DestinationWalletId.HasValue)
            {
                wallet.Withdraw(notification.Amount);

                var destinationWallet = await _walletRepository.GetByIdAsync(notification.DestinationWalletId.Value, cancellationToken);
                if (destinationWallet is not null)
                {
                    destinationWallet.Deposit(notification.Amount);
                    _walletRepository.Update(destinationWallet);
                }
            }

            _walletRepository.Update(wallet);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
