using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.Contracts
{
    public interface IBudgetReadService
    {
        Task<IReadOnlyList<BudgetPeriodDto>> GetUserBudgetsAsync(
            Guid userId,
            DateTime fromDate,
            DateTime toDate,
            CancellationToken cancellationToken);
    }


     public record BudgetPeriodDto(
        Guid Id,
        Guid UserId,
        int CategoryId,
        decimal Amount);
}
