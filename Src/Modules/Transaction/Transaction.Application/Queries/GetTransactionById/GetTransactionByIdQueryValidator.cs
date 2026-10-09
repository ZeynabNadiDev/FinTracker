using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Application.Queries.GetTransactionById
{
    public class GetTransactionByIdQueryValidator : AbstractValidator<GetTransactionByIdQuery>
    {
        public GetTransactionByIdQueryValidator()
        {
            RuleFor(x => x.TransactionId)
                .NotEmpty().WithMessage("TransactionId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }
    }
}
