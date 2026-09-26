
using Transaction.Domain.UOW;
using Transaction.Infrastructure.Persistence.DBcontext;

namespace Transaction.Infrastructure.Persistence.Uow
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly TransactionDbContext _context;

        public UnitOfWork(TransactionDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
