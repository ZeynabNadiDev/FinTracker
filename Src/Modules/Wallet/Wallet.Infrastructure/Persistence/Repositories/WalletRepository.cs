using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.Repository;
using Wallet.Infrastructure.Persistence.DBcontext;

namespace Wallet.Infrastructure.Persistence.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext _context;

        public WalletRepository(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Wallet.Wallet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Wallets
                .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }

        public async Task<List<Domain.Entities.Wallet.Wallet>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Wallets
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync(cancellationToken);
        }
        public async Task<bool> ExistsByTitleAsync(Guid userId, string title, CancellationToken cancellationToken = default)
        {
            return await _context.Wallets
                .AnyAsync(w => w.UserId == userId && w.Title == title && !w.IsRemoved, cancellationToken);
        }
        public async Task<bool> ExistsByTitleAsync(Guid userId, string title, Guid excludedWalletId, CancellationToken cancellationToken = default)
        {
            return await _context.Wallets
                .AnyAsync(w => w.UserId == userId && w.Title == title && w.Id != excludedWalletId && !w.IsRemoved, cancellationToken);
        }


        public async Task AddAsync(Domain.Entities.Wallet.Wallet wallet, CancellationToken cancellationToken = default)
        {
            await _context.Wallets.AddAsync(wallet, cancellationToken);
        }

        public void Update(Domain.Entities.Wallet.Wallet wallet)
        {
            _context.Wallets.Update(wallet);
        }

        public void Delete(Domain.Entities.Wallet.Wallet wallet)
        {
            _context.Wallets.Remove(wallet);
        }
    }
}
