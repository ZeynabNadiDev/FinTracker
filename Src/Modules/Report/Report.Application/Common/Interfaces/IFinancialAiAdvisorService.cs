using Report.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Common.Interfaces
{
    public interface IFinancialAiAdvisorService
    {
        Task<string> GenerateFinancialAdviceAsync(
         decimal totalIncome,
         decimal totalExpense,
         IReadOnlyList<CategoryExpenseDto> categories,
         IReadOnlyList<BudgetAlertDto> budgetAlerts,
         CancellationToken cancellationToken = default);
    }
}
