using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.Contracts
{
    public interface IWalletContract
    {
        Task<bool> IsWalletOwnedByUserAsync(Guid walletId, Guid userId, CancellationToken cancellationToken = default);

        Task<WalletDto?> GetWalletByIdAsync(
            Guid walletId,
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<WalletDto>> GetUserWalletsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
    }

      public sealed record WalletDto(
        Guid Id,
        Guid UserId,
        string Title,
        string Currency,
        decimal Balance
);
}
