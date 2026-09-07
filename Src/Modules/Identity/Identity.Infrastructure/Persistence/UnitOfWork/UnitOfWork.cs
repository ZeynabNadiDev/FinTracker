using Identity.Domain.UnitOfWork;
using Identity.Infrastructure.Persistence.DBcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly IdentityDbContext _context;

        public UnitOfWork(IdentityDbContext context)
        {
            _context = context;
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
