using FinTracker.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinTracker.SharedKernel.Contracts
{
    public interface ITransactionContract
    {
        Task<IReadOnlyList<TransactionPeriodDto>> GetTransactionsByPeriodAsync(
            Guid userId,
            DateOnly startDate,
            DateOnly endDate,
            Guid? walletId = null,
            CancellationToken cancellationToken = default);
    }

    public record TransactionPeriodDto(
       Guid Id,
       Guid UserId,
       Guid WalletId,
       int? CategoryId,
       decimal Amount,
       TransactionType Type,
       DateTime TransactionDate);

   
}