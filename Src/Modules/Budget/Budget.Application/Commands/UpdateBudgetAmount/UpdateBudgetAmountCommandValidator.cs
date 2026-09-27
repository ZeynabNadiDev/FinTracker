using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Application.Commands.UpdateBudgetAmount
{
    public class UpdateBudgetAmountCommandValidator : AbstractValidator<UpdateBudgetAmountCommand>
    {
        public UpdateBudgetAmountCommandValidator()
        {
            RuleFor(x => x.BudgetId)
                .NotEmpty().WithMessage("BudgetId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.NewTargetAmount)
                .GreaterThan(0).WithMessage("New target amount must be greater than zero.");
        }
    }
}
