using Category.Application.DTO;
using Category.Domain.Repository;
using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Application.Queries.GetCategoriesByUser.Handler
{
    public sealed class GetCategoriesByUserQueryHandler : IRequestHandler<GetCategoriesByUserQuery, Result<IReadOnlyList<CategoryResponseDto>>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesByUserQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<IReadOnlyList<CategoryResponseDto>>> Handle(
            GetCategoriesByUserQuery request,
            CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            var response = categories.Select(c => new CategoryResponseDto(
                c.Id,
                c.Name,
                c.Description,
                c.UserId,
                c.Type
            )).ToList();

            return Result<IReadOnlyList<CategoryResponseDto>>.Success(response);

        }
    }
}
