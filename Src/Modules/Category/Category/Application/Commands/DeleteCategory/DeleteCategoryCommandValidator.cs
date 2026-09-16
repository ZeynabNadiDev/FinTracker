using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Application.Commands.DeleteCategory
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(c => c.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");

            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
