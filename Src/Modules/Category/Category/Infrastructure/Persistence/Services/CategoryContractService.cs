using Category.Domain.Repository;
using FinTracker.SharedKernel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Category.Infrastructure.Persistence.Services
{
    public sealed class CategoryContractService : ICategoryContract
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryContractService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> IsCategoryAccessibleByUserAsync(
            int categoryId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (category is null || category.IsRemoved)
            {
                return false;
            }

            return category.UserId == userId;
        }

        public async Task<IReadOnlyDictionary<int, ICategoryContract.CategoryDto>> GetCategoriesByIdsAsync(
            IEnumerable<int> categoryIds,
            CancellationToken cancellationToken = default)
        {
            var distinctIds = categoryIds?.Distinct().ToList() ?? new List<int>();
            var result = new Dictionary<int, ICategoryContract.CategoryDto>();

            if (distinctIds.Count == 0)
            {
                return result;
            }

            // Sequential fetch to avoid EF Core DbContext concurrency issues
            foreach (var id in distinctIds)
            {
                var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
                if (category is not null && !category.IsRemoved)
                {
                    result[category.Id] = new ICategoryContract.CategoryDto(category.Id, category.Name);
                }
            }

            return result;
        }
    }
}
