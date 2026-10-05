using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.Repository;

namespace Wallet.Application.Commands.TransferWallet
{
    public class TransferWalletCommandValidator : AbstractValidator<TransferWalletCommand>
    {
        private readonly IWalletRepository _walletRepository;

        public TransferWalletCommandValidator(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;

            RuleFor(x => x.SourceWalletId)
                .NotEmpty()
                .WithMessage("Source wallet ID is required.");

            RuleFor(x => x.DestinationWalletId)
                .NotEmpty()
                .WithMessage("Destination wallet ID is required.")
                .NotEqual(x => x.SourceWalletId)
                .WithMessage("Destination wallet cannot be the same as the source wallet.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Transfer amount must be greater than zero.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");

            // Validate source wallet ownership
            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) =>
                {
                    var sourceWallet = await _walletRepository.GetByIdAsync(command.SourceWalletId, cancellationToken);
                    return sourceWallet is not null && sourceWallet.UserId == command.UserId;
                })
                .WithMessage("Source wallet was not found or does not belong to the user.")
                .WithName("SourceWalletId");

            // Validate destination wallet ownership
            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) =>
                {
                    var destWallet = await _walletRepository.GetByIdAsync(command.DestinationWalletId, cancellationToken);
                    return destWallet is not null && destWallet.UserId == command.UserId;
                })
                .WithMessage("Destination wallet was not found or does not belong to the user.")
                .WithName("DestinationWalletId");
        }
    }
}
