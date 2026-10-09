using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Application.Commands.UpdateBudgetAmount
{
    public record UpdateBudgetAmountCommand(
      Guid BudgetId,
      Guid UserId,
      decimal NewTargetAmount
  ) : IRequest<Result>;
}
