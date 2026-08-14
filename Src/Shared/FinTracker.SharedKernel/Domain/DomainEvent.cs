using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.Domain
{
    public abstract record DomainEvent:IDomainEvent
    {
        public Guid AggregateId { get; }
        public long AggregateVersion { get; }
        public DateTime OccurredOn { get; }

        protected DomainEvent(Guid aggregateId, long aggregateVersion)
        {
            AggregateId = aggregateId;
            AggregateVersion = aggregateVersion;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
