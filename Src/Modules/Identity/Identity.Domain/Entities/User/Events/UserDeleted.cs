using FinTracker.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities.User.Events
{
    public sealed record UserDeleted: DomainEvent<Guid>
    {
        public UserDeleted(User user)
            : base(user.Id, user.Version)
        {
        }
  
    }
}
