using Category.Application.DTO;
using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Application.Queries.GetCategoriesByUser
{
    public sealed record GetCategoriesByUserQuery(Guid UserId) : IRequest<Result<IReadOnlyList<CategoryResponseDto>>>;
}
