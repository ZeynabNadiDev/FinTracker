using Budget.Domain.Repositories;
using Budget.Infrastructure.Persistence.DBcontext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Persistence.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly BudgetDbContext _context;

        public BudgetRepository(BudgetDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.Entities.Budget.Budget budget, CancellationToken cancellationToken = default)
        {
            await _context.Budgets.AddAsync(budget, cancellationToken);
        }

        public async Task<Domain.Entities.Budget.Budget?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Budgets
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<Domain.Entities.Budget.Budget?> GetByUserCategoryAndPeriodAsync(
            Guid userId,
            int categoryId,
            int month,
            int year,
            CancellationToken cancellationToken = default)
        {
            return await _context.Budgets
                .FirstOrDefaultAsync(b =>
                    b.UserId == userId &&
                    b.CategoryId == categoryId &&
                    b.Month == month &&
                    b.Year == year,
                    cancellationToken);
        }

        public async Task<IReadOnlyList<Domain.Entities.Budget.Budget>> GetByUserAndPeriodAsync(
            Guid userId,
            int month,
            int year,
            CancellationToken cancellationToken = default)
        {
            return await _context.Budgets
                .AsNoTracking()
                .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
                .ToListAsync(cancellationToken);
        }

        public void Update(Domain.Entities.Budget.Budget budget)
        {
            _context.Budgets.Update(budget);
        }
    }
}
