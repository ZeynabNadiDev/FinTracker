using Category.Domain.Uow;
using Category.Infrastructure.Persistence.DBcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Infrastructure.Persistence.Uow
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly CategoryDbContext _context;

        public UnitOfWork(CategoryDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
