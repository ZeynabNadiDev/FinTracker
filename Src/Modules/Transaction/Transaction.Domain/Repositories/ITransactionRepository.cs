using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction.Domain.Entities.Transaction.Transaction transaction, CancellationToken cancellationToken = default);
        Task<Transaction.Domain.Entities.Transaction.Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Transaction.Domain.Entities.Transaction.Transaction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Transaction.Domain.Entities.Transaction.Transaction>> GetByWalletIdAsync(Guid walletId, CancellationToken cancellationToken = default);
        Task<(IReadOnlyList<Transaction.Domain.Entities.Transaction.Transaction> Items, int TotalCount)> GetPagedByUserIdAsync(
            Guid userId,int pageNumber,int pageSize,Guid? walletId,int? categoryId,DateTime? fromDate,DateTime? toDate,
            CancellationToken cancellationToken);
        void Update(Transaction.Domain.Entities.Transaction.Transaction transaction);
    }
}
