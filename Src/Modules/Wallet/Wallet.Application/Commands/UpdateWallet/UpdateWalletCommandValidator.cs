using FluentValidation;

namespace Wallet.Application.Commands.UpdateWallet
{
    public sealed class UpdateWalletCommandValidator
     : AbstractValidator<UpdateWalletCommand>
    {
        public UpdateWalletCommandValidator()
        {
            RuleFor(command => command.WalletId)
                .NotEmpty()
                .WithMessage("Wallet ID is required.");

            RuleFor(command => command.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");

            RuleFor(command => command.Title)
                .NotEmpty()
                .WithMessage("Wallet title is required.")
                .MaximumLength(100)
                .WithMessage("Wallet title must not exceed 100 characters.");
        }
    }
}
