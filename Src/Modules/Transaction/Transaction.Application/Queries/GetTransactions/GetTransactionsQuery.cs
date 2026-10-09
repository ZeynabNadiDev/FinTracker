using FinTracker.SharedKernel.Pagination;
using FinTracker.SharedKernel.Results;
using MediatR;
using Transaction.Application.DTOs;

namespace Transaction.Application.Queries.GetTransactions
{
    public record GetTransactionsQuery(
      Guid UserId,
      int PageNumber = 1,
      int PageSize = 10,
      Guid? WalletId = null,
      int? CategoryId = null,
      DateTime? FromDate = null,
      DateTime? ToDate = null
  ) : IRequest<Result<PagedResult<TransactionDto>>>;
}
