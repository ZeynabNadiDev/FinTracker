using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Application.DTOs
{
    public record BudgetDto(
      Guid Id,
      int CategoryId,
      decimal TargetAmount,
      int Month,
      int Year
  );
    public record CreateBudgetRequest(int CategoryId, decimal Amount, int Month, int Year);
    public record UpdateBudgetAmountRequest(decimal Amount);
}
