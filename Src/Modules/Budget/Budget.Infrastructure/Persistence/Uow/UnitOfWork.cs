using Budget.Domain.UOW;
using Budget.Infrastructure.Persistence.DBcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Persistence.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BudgetDbContext _context;

        public UnitOfWork(BudgetDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
