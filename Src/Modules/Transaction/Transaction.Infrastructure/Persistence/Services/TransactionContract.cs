
using FinTracker.SharedKernel.Contracts;
using FinTracker.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transaction.Domain.Repositories;

namespace Transaction.Infrastructure.Persistence.Services
{
    public sealed class TransactionContract : ITransactionContract
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionContract(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IReadOnlyList<TransactionPeriodDto>> GetTransactionsByPeriodAsync(
            Guid userId,
            DateOnly startDate,
            DateOnly endDate,
            Guid? walletId = null,
            CancellationToken cancellationToken = default)
        {
            var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
            var endDateTime = endDate.ToDateTime(TimeOnly.MaxValue);

            // Fetch transactions for the user within the period using the existing repository method
            var (items, _) = await _transactionRepository.GetPagedByUserIdAsync(
                userId: userId,
                pageNumber: 1,
                pageSize: int.MaxValue,
                walletId: walletId,
                categoryId: null,
                fromDate: startDateTime,
                toDate: endDateTime,
                cancellationToken: cancellationToken);

            return items
                .Select(t => new TransactionPeriodDto(
                    t.Id,
                    t.UserId,
                    t.WalletId,
                    t.CategoryId,
                    t.Amount,
                    (TransactionType)t.Type,
                    t.TransactionDate))
                .ToList();
        }
    }
}
