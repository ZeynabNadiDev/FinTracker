using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Application.Queries.GetBudgets
{
    public class GetBudgetsQueryValidator : AbstractValidator<GetBudgetsQuery>
    {
        public GetBudgetsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

            RuleFor(x => x.Year)
                .GreaterThanOrEqualTo(2000).WithMessage("Year is invalid.");
        }
    }
}
