using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Application.Commands.CreateBudget
{
    public record CreateBudgetCommand(
    Guid UserId,
    int CategoryId,
    decimal TargetAmount,
    int Month,
    int Year
) : IRequest<Result<Guid>>;
}
