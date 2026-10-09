
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
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }


    }
}
