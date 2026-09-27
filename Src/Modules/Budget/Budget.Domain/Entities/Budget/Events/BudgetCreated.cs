using FinTracker.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
