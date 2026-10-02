using Budget.Domain.Repositories;
using FinTracker.SharedKernel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Persistence.Services
{
    public sealed class BudgetReadService : IBudgetReadService
    {
        private readonly IBudgetRepository _budgetRepository;

        public BudgetReadService(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<IReadOnlyList<BudgetPeriodDto>> GetUserBudgetsAsync(
            Guid userId,
            DateTime fromDate,
            DateTime toDate,
            CancellationToken cancellationToken)
        {
            var result = new List<BudgetPeriodDto>();

            // Iterate through all (year, month) pairs within the range
            var current = new DateTime(fromDate.Year, fromDate.Month, 1);
            var end = new DateTime(toDate.Year, toDate.Month, 1);

            while (current <= end)
            {
                var monthlyBudgets = await _budgetRepository.GetByUserAndPeriodAsync(
                    userId,
                    current.Month,
                    current.Year,
                    cancellationToken);

                if (monthlyBudgets != null && monthlyBudgets.Count > 0)
                {
                    result.AddRange(monthlyBudgets.Select(b => new BudgetPeriodDto(
                        b.Id,
                        b.UserId,
                        b.CategoryId,
                        b.TargetAmount)));
                }

                current = current.AddMonths(1);
            }

            return result;
        }
    }
}
