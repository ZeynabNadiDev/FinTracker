using FluentValidation;


namespace Transaction.Application.Commands.CreateTransaction
{
    public sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(x => x.WalletId)
                .NotEmpty()
                .WithMessage("WalletId is required.");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("CategoryId is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid transaction type.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

           
        }
    }
}
