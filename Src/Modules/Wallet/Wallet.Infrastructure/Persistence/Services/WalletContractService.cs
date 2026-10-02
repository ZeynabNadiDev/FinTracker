using FinTracker.SharedKernel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Domain.Repository;

namespace Wallet.Infrastructure.Persistence.Services
{
    public sealed class WalletContractService : IWalletContract
    {
        private readonly IWalletRepository _walletRepository;

        public WalletContractService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<bool> IsWalletOwnedByUserAsync(
            Guid walletId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId, cancellationToken);
            if (wallet is null)
            {
                return false;
            }

            return wallet.UserId == userId;
        }

        public async Task<WalletDto?> GetWalletByIdAsync(
            Guid walletId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId, cancellationToken);
            if (wallet is null || wallet.UserId != userId)
            {
                return null;
            }

            return new WalletDto(
                wallet.Id,
                wallet.UserId,
                wallet.Title,
                wallet.Balance.Currency,
                wallet.Balance.Amount
            );
        }

        public async Task<IReadOnlyList<WalletDto>> GetUserWalletsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var wallets = await _walletRepository.GetAllByUserIdAsync(userId, cancellationToken);

            return wallets.Select(w => new WalletDto(
                w.Id,
                w.UserId,
                w.Title,
                w.Balance.Currency,
                w.Balance.Amount
            )).ToList();
        }
    }
}
