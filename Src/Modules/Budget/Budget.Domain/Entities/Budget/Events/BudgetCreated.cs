using FinTracker.SharedKernel.Domain;
using System;

namespace Budget.Domain.Entities.Budget.Events
{
    public sealed record BudgetCreated : DomainEvent<Guid>
    {
        public BudgetCreated(Budget budget) : base(budget.Id, budget.Version) { }
    }

    public sealed record BudgetAmountUpdated : DomainEvent<Guid>
    {
        public BudgetAmountUpdated(Budget budget) : base(budget.Id, budget.Version) { }
    }

    public sealed record BudgetThresholdReached : DomainEvent<Guid>
    {
        public decimal ThresholdPercentage { get; }
        public decimal CurrentSpent { get; }

        public BudgetThresholdReached(Budget budget, decimal thresholdPercentage, decimal currentSpent)
            : base(budget.Id, budget.Version)
        {
            ThresholdPercentage = thresholdPercentage;
            CurrentSpent = currentSpent;
        }
    }

    public sealed record BudgetExceeded : DomainEvent<Guid>
    {
        public decimal TotalSpent { get; }

        public BudgetExceeded(Budget budget, decimal totalSpent)
            : base(budget.Id, budget.Version)
        {
            TotalSpent = totalSpent;
        }
    }
}
