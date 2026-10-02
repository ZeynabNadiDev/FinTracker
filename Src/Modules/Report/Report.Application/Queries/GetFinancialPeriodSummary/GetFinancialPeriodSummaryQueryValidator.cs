using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Queries.GetFinancialPeriodSummary
{
    public class GetFinancialPeriodSummaryQueryValidator : AbstractValidator<GetFinancialPeriodSummaryQuery>
    {
        public GetFinancialPeriodSummaryQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User identifier is required.");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("End date is required.")
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be greater than or equal to start date.");

            When(x => x.WalletId.HasValue, () =>
            {
                RuleFor(x => x.WalletId!.Value)
                .NotEmpty()
                .WithMessage("Wallet identifier is required.");
            });
        }
    }
}
