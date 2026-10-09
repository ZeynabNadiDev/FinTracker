using Category.Application.DTO;
using Category.Domain.Enums;
using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Application.Commands.CreateCategory
{
    public sealed record CreateCategoryCommand(
     string Name,
     string? Description,
     Guid UserId,
     CategoryType Type) : IRequest<Result<CategoryResponseDto>>;
}
