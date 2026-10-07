using Microsoft.EntityFrameworkCore;
using Notification.Domain.UOW;
using Notification.Domain.Repositories;
using Notification.Infrastructure.Persistence.DBcontext;
using NotificationEntity = Notification.Domain.Entities.Notification.Notification;

namespace Notification.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NotificationEntity notification, CancellationToken cancellationToken = default)
        {
            await _context.Notifications.AddAsync(notification, cancellationToken);
        }

        public async Task<NotificationEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<NotificationEntity>> GetByUserIdAsync(
            Guid userId,
            bool onlyUnread = false,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId);

            if (onlyUnread)
            {
                query = query.Where(n => !n.IsRead);
            }

            return await query
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetUnreadCountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Notifications
                .AsNoTracking()
                .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);
        }

        public async Task<IReadOnlyList<NotificationEntity>> GetUnreadByUserIdAsync(Guid userId,CancellationToken cancellationToken = default)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<NotificationEntity>> GetByUserIdAsync(
            Guid userId,
            bool onlyUnread,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Notifications.AsNoTracking();

            if (onlyUnread)
                query = query.Where(n => !n.IsRead);

            return await query
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }


    }
}
