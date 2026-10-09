using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Application.Queries.GetCategoriesByUser
{
    public sealed class GetCategoriesByUserQueryValidator : AbstractValidator<GetCategoriesByUserQuery>
    {
        public GetCategoriesByUserQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .NotEqual(Guid.Empty).WithMessage("UserId cannot be empty.");
        }
    }
}
