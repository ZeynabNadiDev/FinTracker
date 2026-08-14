using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.Domain
{
    public interface IDomainEvent
    {
        Guid AggregateId { get; }

        long AggregateVersion { get; }

        DateTime OccurredOn { get; }
    }
}
