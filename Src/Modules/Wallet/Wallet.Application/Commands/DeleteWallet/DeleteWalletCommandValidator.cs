using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Commands.DeleteWallet
{
    public class DeleteWalletCommandValidator : AbstractValidator<DeleteWalletCommand>
    {
        public DeleteWalletCommandValidator()
        {
            RuleFor(x => x.WalletId)
                .NotEmpty()
                .WithMessage("Wallet ID is required.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");
        }
    }
}
