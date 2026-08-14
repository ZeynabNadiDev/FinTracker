using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.Domain
{
    public abstract class AggregateRoot<TKey> : BaseEntity<TKey>
    {
       public long Version { get; private set; }
        private readonly List<IDomainEvent> _domainEvents = new();
        protected AggregateRoot()
        {
        }

        protected AggregateRoot(TKey id)
            : base(id)
        {
        }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
