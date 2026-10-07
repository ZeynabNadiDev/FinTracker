using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Domain.UOW;
using Notification.Infrastructure.Persistence.DBcontext;

namespace Notification.Infrastructure.Persistence.Uow
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly NotificationDbContext _context;
        private bool _disposed;

        public UnitOfWork(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

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
