using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.Domain
{
    public abstract class BaseEntity<TKey>
    {
        protected BaseEntity()
        {
        }

        protected BaseEntity(TKey id)
        {
            Id = id;
        }

        public TKey Id { get; private set; }
    }
}
