using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Repositories
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification.Domain.Entities.Notification.Notification notification, CancellationToken cancellationToken = default);
        Task<Notification.Domain.Entities.Notification.Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Notification.Domain.Entities.Notification.Notification>> GetByUserIdAsync(Guid userId, bool onlyUnread = false, CancellationToken cancellationToken = default);
        Task<int> GetUnreadCountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Notification.Domain.Entities.Notification.Notification>> GetUnreadByUserIdAsync(Guid userId,
         CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Notification.Domain.Entities.Notification.Notification>> GetByUserIdAsync(
            Guid userId,
            bool onlyUnread,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

    }
}
