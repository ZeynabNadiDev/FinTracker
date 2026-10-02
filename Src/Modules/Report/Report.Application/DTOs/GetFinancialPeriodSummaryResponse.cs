using System;
using System.Collections.Generic;

namespace Report.DTOs
{
    public sealed record GetFinancialPeriodSummaryResponse
    {
        public DateOnly FromDate { get; init; }
        public DateOnly ToDate { get; init; }
        public decimal TotalIncome { get; init; }
        public decimal TotalExpense { get; init; }
        public decimal NetSavings => TotalIncome - TotalExpense;
        public decimal SavingsRate => TotalIncome > 0 ? Math.Round((NetSavings / TotalIncome) * 100, 2) : 0;

        public TopExpenseCategoryDto? TopExpenseCategory { get; init; }
        public IReadOnlyList<CategoryExpenseDto> CategoryExpenses { get; init; } = [];
        public IReadOnlyList<BudgetAlertDto> BudgetExceededAlerts { get; init; } = [];
        public IReadOnlyList<string> AiAdviceList { get; init; } = [];
    }

    public sealed record CategoryExpenseDto(int CategoryId, string CategoryName, decimal TotalAmount, decimal PercentageOfTotalExpense);

    public sealed record TopExpenseCategoryDto(int CategoryId, string CategoryName, decimal TotalAmount);

    public sealed record BudgetAlertDto(int CategoryId, string CategoryName, decimal BudgetLimit, decimal ActualSpent, decimal ExceededAmount);
}
