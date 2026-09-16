using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Application.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(int CategoryId, Guid UserId) : IRequest<Result>;
}
