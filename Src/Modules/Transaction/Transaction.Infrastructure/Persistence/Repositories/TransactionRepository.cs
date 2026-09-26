using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transaction.Domain.Entities.Transaction;
using Transaction.Domain.Repositories;
using Transaction.Infrastructure.Persistence.DBcontext;
using TransactionEntity = Transaction.Domain.Entities.Transaction.Transaction;

namespace Transaction.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDbContext _context;

        public TransactionRepository(TransactionDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TransactionEntity transaction, CancellationToken cancellationToken = default)
        {
            await _context.Transactions.AddAsync(transaction, cancellationToken);
        }

        public async Task<TransactionEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsRemoved, cancellationToken);
        }

        public async Task<IReadOnlyList<TransactionEntity>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId && !t.IsRemoved)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TransactionEntity>> GetByWalletIdAsync(Guid walletId, CancellationToken cancellationToken = default)
        {
            return await _context.Transactions
                .Where(t => t.WalletId == walletId && !t.IsRemoved)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<(IReadOnlyList<TransactionEntity> Items, int TotalCount)> GetPagedByUserIdAsync(
            Guid userId,
            int pageNumber,
            int pageSize,
            Guid? walletId,
            int? categoryId,
            DateTime? fromDate,
            DateTime? toDate,
            CancellationToken cancellationToken)
        {
            var query = _context.Transactions
                .AsNoTracking()
                .Where(t => t.UserId == userId && !t.IsRemoved);

            if (walletId.HasValue)
                query = query.Where(t => t.WalletId == walletId.Value);

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId.Value);

            if (fromDate.HasValue)
                query = query.Where(t => t.TransactionDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.TransactionDate <= toDate.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(t => t.TransactionDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public void Update(TransactionEntity transaction)
        {
            _context.Transactions.Update(transaction);
        }
    }
}
