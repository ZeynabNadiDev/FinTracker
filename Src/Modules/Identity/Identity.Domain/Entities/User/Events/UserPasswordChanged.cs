using FinTracker.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities.User.Events
{
    public sealed record UserPasswordChanged : DomainEvent<Guid>
    {
        public UserPasswordChanged(User user)
               : base(user.Id, user.Version)
        {
        }


    }

}