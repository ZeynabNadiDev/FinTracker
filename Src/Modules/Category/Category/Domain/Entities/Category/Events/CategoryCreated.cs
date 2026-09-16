using FinTracker.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Domain.Entities.Category.Events
{
    public sealed record CategoryCreated : DomainEvent<int>
    {
        public CategoryCreated(Category category)
          : base(category.Id, category.Version)
        {
        }
    }

    public sealed record CategoryUpdated : DomainEvent<int>
    {
        public CategoryUpdated(Category category)
          : base(category.Id, category.Version)
        {
        }
    }

    public sealed record CategoryDeleted : DomainEvent<int>
    {
        public CategoryDeleted(Category category)
          : base(category.Id, category.Version)
        {
        }
    }
}
