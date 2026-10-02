using FinTracker.SharedKernel.Contracts;
using MediatR;
using Report.Common.Interfaces;
using Report.DTOs;
using Report.Queries.GetFinancialPeriodSummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transaction.Infrastructure.Persistence.Services;

namespace Report.Queries.GetFinancialPeriodSummary
{
    public sealed class GetFinancialPeriodSummaryQueryHandler
        : IRequestHandler<GetFinancialPeriodSummaryQuery, GetFinancialPeriodSummaryResponse>
    {
        private readonly ITransactionContract _transactionContract;
        private readonly IBudgetReadService _budgetReadService;
        private readonly ICategoryContract _categoryContract;
        private readonly IFinancialAiAdvisorService _aiAdvisorService;

        public GetFinancialPeriodSummaryQueryHandler(
            ITransactionContract transactionContract,
            IBudgetReadService budgetReadService,
            ICategoryContract categoryContract,
            IFinancialAiAdvisorService aiAdvisorService)
        {
            _transactionContract = transactionContract;
            _budgetReadService = budgetReadService;
            _categoryContract = categoryContract;
            _aiAdvisorService = aiAdvisorService;
        }

        public async Task<GetFinancialPeriodSummaryResponse> Handle(
            GetFinancialPeriodSummaryQuery request,
            CancellationToken cancellationToken)
        {
            var transactions = await _transactionContract.GetTransactionsByPeriodAsync(
                request.UserId, request.StartDate, request.EndDate, request.WalletId, cancellationToken);

            var totalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var totalExpense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

            var expenseTransactions = transactions.Where(t => t.Type == TransactionType.Expense).ToList();
            var categoryIds = expenseTransactions.Select(t => t.CategoryId).Distinct().ToList();

            var categoriesTask = _categoryContract.GetCategoriesByIdsAsync(categoryIds, cancellationToken);
            var budgetsTask = _budgetReadService.GetUserBudgetsAsync(
                request.UserId, request.StartDate.ToDateTime(TimeOnly.MinValue), request.EndDate.ToDateTime(TimeOnly.MaxValue), cancellationToken);

            await Task.WhenAll(categoriesTask, budgetsTask);

            var categoriesMap = await categoriesTask;
            var budgets = await budgetsTask;

            // Prepare category expenses
            var categoryExpenses = expenseTransactions
                .GroupBy(t => t.CategoryId)
                .Select(g => {
                    var catName = categoriesMap.TryGetValue(g.Key, out var cat) ? cat.Name : "Unknown";
                    var spent = g.Sum(t => t.Amount);
                    return new CategoryExpenseDto(g.Key, catName, spent, totalExpense > 0 ? (spent / totalExpense) * 100 : 0);
                }).ToList();

            // Find top expense category
            var topCategory = categoryExpenses.OrderByDescending(c => c.TotalAmount).FirstOrDefault();
            var topExpenseDto = topCategory != null ? new TopExpenseCategoryDto(topCategory.CategoryId, topCategory.CategoryName, topCategory.TotalAmount) : null;

            // Prepare budget alerts
            var alerts = budgets
                .Where(b => expenseTransactions.Any(et => et.CategoryId == b.CategoryId))
                .Select(b => {
                    var spent = expenseTransactions.Where(et => et.CategoryId == b.CategoryId).Sum(et => et.Amount);
                    return new { Budget = b, Spent = spent };
                })
                .Where(x => x.Spent > x.Budget.Amount)
                .Select(x => {
                    var catName = categoriesMap.TryGetValue(x.Budget.CategoryId, out var cat) ? cat.Name : "Unknown";
                    return new BudgetAlertDto(x.Budget.CategoryId, catName, x.Budget.Amount, x.Spent, x.Spent - x.Budget.Amount);
                }).ToList();

            // Generate AI Advice
            var aiAdviceString = await _aiAdvisorService.GenerateFinancialAdviceAsync(
              totalIncome,
              totalExpense,
              new List<CategoryExpenseDto>(),
              alerts,
               cancellationToken);

            return new GetFinancialPeriodSummaryResponse
            {
                FromDate = request.StartDate,
                ToDate = request.EndDate,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                CategoryExpenses = categoryExpenses,
                TopExpenseCategory = topExpenseDto,
                BudgetExceededAlerts = alerts,
                AiAdviceList = new List<string> { aiAdviceString } // Wrapping in list to match your DTO
            };
        }
    }
}
