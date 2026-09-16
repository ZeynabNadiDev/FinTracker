using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.Domain
{
    public abstract record DomainEvent<TId>:IDomainEvent<TId>
    {
        public TId AggregateId { get; }
        public long AggregateVersion { get; }
        public DateTime OccurredOn { get; }

        protected DomainEvent(TId aggregateId, long aggregateVersion)
        {
            AggregateId = aggregateId;
            AggregateVersion = aggregateVersion;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
