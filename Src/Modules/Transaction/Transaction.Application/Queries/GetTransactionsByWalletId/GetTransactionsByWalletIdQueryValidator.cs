using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Application.Queries.GetTransactionsByWalletId
{
    public class GetTransactionsByWalletIdQueryValidator : AbstractValidator<GetTransactionsByWalletIdQuery>
    {
        public GetTransactionsByWalletIdQueryValidator()
        {
            RuleFor(x => x.WalletId)
                .NotEmpty().WithMessage("WalletId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }
    }
}
