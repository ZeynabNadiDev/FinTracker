using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Queries.GetWalletsByUserId
{
    public class GetWalletsByUserIdQueryValidator : AbstractValidator<GetWalletsByUserIdQuery>
    {
        public GetWalletsByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");
        }
    }
}
