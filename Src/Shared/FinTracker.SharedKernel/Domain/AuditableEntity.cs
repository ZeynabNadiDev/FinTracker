using FinTracker.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.Domain
{
    public class AuditableEntity<TKey> : BaseEntity<TKey>
    {
        protected AuditableEntity() { }
        protected AuditableEntity(TKey id)
            : base(id)
        {
        }

        public DateTime CreatedAt { get; protected set; }

        public DateTime? ModifiedAt { get; protected set; }

        public DateTime? DeletedAt { get; protected set; }
    }
}
