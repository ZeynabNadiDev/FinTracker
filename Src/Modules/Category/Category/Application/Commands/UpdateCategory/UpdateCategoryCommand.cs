using Category.Application.DTO;
using Category.Domain.Enums;
using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Application.Commands.UpdateCategory
{
    public sealed record UpdateCategoryCommand(
     int CategoryId,
     Guid UserId,
     string Name,
     string? Description,
     CategoryType Type
 ) : IRequest<Result<CategoryResponseDto>>;
}
