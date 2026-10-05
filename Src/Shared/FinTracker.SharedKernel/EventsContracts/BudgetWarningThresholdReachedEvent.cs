using FinTracker.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.EventsContracts
{
    public sealed record BudgetWarningThresholdReachedEvent(
    Guid BudgetId,
    Guid UserId,
    int CategoryId,
    decimal TargetAmount,
    decimal CurrentSpentAmount,
    decimal Percentage) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public long AggregateVersion { get; init; } = 1;
    }

    public sealed record BudgetExceededEvent(
        Guid BudgetId,
        Guid UserId,
        int CategoryId,
        decimal TargetAmount,
        decimal CurrentSpentAmount,
        decimal Percentage) : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public long AggregateVersion { get; init; } = 1;
    }
}
