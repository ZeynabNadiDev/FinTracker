using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.Domain
{
    public interface IDomainEvent
    {
        long AggregateVersion { get; }

        DateTime OccurredOn { get; }
    }

    public interface IDomainEvent<out TId> : IDomainEvent
    {
        TId AggregateId { get; }
    }
}
