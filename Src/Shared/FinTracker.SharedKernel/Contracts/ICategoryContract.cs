using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.Contracts
{
    public interface ICategoryContract
    {
        Task<bool> IsCategoryAccessibleByUserAsync(int categoryId, Guid userId, CancellationToken cancellationToken = default);

        Task<IReadOnlyDictionary<int, CategoryDto>> GetCategoriesByIdsAsync(
        IEnumerable<int> categoryIds,
        CancellationToken cancellationToken = default);

        public record CategoryDto(int Id, string Name);
    }
}
