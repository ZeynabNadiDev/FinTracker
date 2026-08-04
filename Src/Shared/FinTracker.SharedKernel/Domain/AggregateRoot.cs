using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.Domain
{
    public abstract class AggregateRoot<TKey> : BaseEntity<TKey>
    {
        protected AggregateRoot()
        {
        }

        protected AggregateRoot(TKey id)
            : base(id)
        {
        }
    }
}
