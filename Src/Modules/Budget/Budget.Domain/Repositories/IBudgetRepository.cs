using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Domain.Repositories
{
    public interface IBudgetRepository
    {
        Task AddAsync(Budget.Domain.Entities.Budget.Budget budget,
        CancellationToken cancellationToken = default);

        Task<Budget.Domain.Entities.Budget.Budget?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<Budget.Domain.Entities.Budget.Budget?> GetByUserCategoryAndPeriodAsync(
            Guid userId,
            int categoryId,
            int month,
            int year,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Budget.Domain.Entities.Budget.Budget>> GetByUserAndPeriodAsync(
            Guid userId,
            int month,
            int year,
            CancellationToken cancellationToken = default);

        void Update(Budget.Domain.Entities.Budget.Budget budget);
    }
}
