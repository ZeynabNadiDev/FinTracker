using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Queries.GetUnreadNotificationCount
{
    public class GetUnreadNotificationCountQueryValidator : AbstractValidator<GetUnreadNotificationCountQuery>
    {
        public GetUnreadNotificationCountQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");
        }
    }
}
