using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Queries.GetNotificationsByUserId
{
    public class GetNotificationsByUserIdQueryValidator : AbstractValidator<GetNotificationsByUserIdQuery>
    {
        public GetNotificationsByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(50);
        }
    }
}
