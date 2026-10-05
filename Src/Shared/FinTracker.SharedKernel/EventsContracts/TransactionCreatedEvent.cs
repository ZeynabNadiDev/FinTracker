using System;
using FinTracker.SharedKernel.Domain;
using FinTracker.SharedKernel.Enums;

namespace FinTracker.SharedKernel.Events
{
    public sealed record TransactionCreatedEvent(
        Guid TransactionId,
        Guid WalletId,
        Guid? DestinationWalletId,
        decimal Amount,
        TransactionType Type,
        int? CategoryId
    ) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public long AggregateVersion { get; init; } = 1;
    }

    public sealed record TransactionDeletedEvent(
        Guid TransactionId,
        Guid WalletId,
        decimal Amount,
        TransactionType Type
    ) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public long AggregateVersion { get; init; } = 1;
    }
}
