using FinTracker.SharedKernel.Domain;
using FinTracker.SharedKernel.Exceptions;
using Notification.Domain.Enums;

namespace Notification.Domain.Entities.Notification
{
    public class Notification : AggregateRoot<Guid>
    {
        public Guid UserId { get; private set; }
        public string Title { get; private set; } = null!;
        public string Message { get; private set; } = null!;
        public NotificationType Type { get; private set; }
        public bool IsRead { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ReadAt { get; private set; }

        protected Notification() : base(Guid.Empty) { }

        private Notification(Guid id, Guid userId, string title, string message, NotificationType type) : base(id)
        {
            EnsureNotEmpty(title, nameof(title));
            EnsureNotEmpty(message, nameof(message));

            if (userId == Guid.Empty)
                throw new DomainException("UserId cannot be empty.");

            UserId = userId;
            Title = title.Trim();
            Message = message.Trim();
            Type = type;
            IsRead = false;
            CreatedAt = DateTime.UtcNow;
        }

        public static Notification Create(Guid id, Guid userId, string title, string message, NotificationType type)
        {
            return new Notification(id, userId, title, message, type);
        }

        public void MarkAsRead()
        {
            if (IsRead) return;

            IsRead = true;
            ReadAt = DateTime.UtcNow;
        }

        private static void EnsureNotEmpty(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException($"{fieldName} cannot be null or empty.");
        }
    }
}
