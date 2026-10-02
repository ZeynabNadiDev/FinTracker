using MediatR;
using Report.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Queries.GetFinancialPeriodSummary
{
    public sealed record GetFinancialPeriodSummaryQuery(
     Guid UserId,
     DateOnly StartDate,
     DateOnly EndDate,
     Guid? WalletId = null) : IRequest<GetFinancialPeriodSummaryResponse>;
}
