using Budget.Application.DTOs;
using Budget.Domain.Repositories;
using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Application.Queries.GetBudgets.Handler
{
    public class GetBudgetsQueryHandler : IRequestHandler<GetBudgetsQuery, Result<List<BudgetDto>>>
    {
        private readonly IBudgetRepository _budgetRepository;

        public GetBudgetsQueryHandler(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<Result<List<BudgetDto>>> Handle(GetBudgetsQuery request, CancellationToken cancellationToken)
        {
            var budgets = await _budgetRepository.GetByUserAndPeriodAsync(
                request.UserId,
                request.Month,
                request.Year,
                cancellationToken);

            var budgetDtos = budgets.Select(b => new BudgetDto(
              b.Id,
              b.CategoryId,
              b.TargetAmount,
              b.Month,
              b.Year
          )).ToList();

            return Result<List<BudgetDto>>.Success(budgetDtos);
        }
    }
}
