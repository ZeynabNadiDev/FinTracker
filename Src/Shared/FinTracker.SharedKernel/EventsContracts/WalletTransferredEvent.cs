using FinTracker.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.Events
{
    public sealed record WalletTransferredEvent(
    Guid SourceWalletId,
    Guid DestinationWalletId,
    Guid UserId,
    decimal Amount
) : IDomainEvent

    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public long AggregateVersion { get; init; } = 1;
    }
}
