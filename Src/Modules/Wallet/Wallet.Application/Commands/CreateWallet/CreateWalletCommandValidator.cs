using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Commands.CreateWallet
{
    public class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
    {
        public CreateWalletCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User identifier is required.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Wallet title is required.")
                .MaximumLength(100)
                .WithMessage("Wallet title must not exceed 100 characters.");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage("Currency code is required.")
                .Length(3)
                .WithMessage("Currency code must be exactly 3 characters (e.g., IRT, USD).");
        }
    }
}
