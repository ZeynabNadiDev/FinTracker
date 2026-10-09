using Budget.Application.DTOs;
using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Application.Queries.GetBudgets
{
    public record GetBudgetsQuery(
    Guid UserId,
    int Month,
    int Year
) : IRequest<Result<List<BudgetDto>>>;
}
