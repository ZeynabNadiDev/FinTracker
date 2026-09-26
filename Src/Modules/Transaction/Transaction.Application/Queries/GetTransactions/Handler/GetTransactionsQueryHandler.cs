using FinTracker.SharedKernel.Pagination;
using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Application.DTOs;
using Transaction.Domain.Repositories;

namespace Transaction.Application.Queries.GetTransactions.Handler
{
    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, Result<PagedResult<TransactionDto>>>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionsQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Result<PagedResult<TransactionDto>>> Handle(
            GetTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _transactionRepository.GetPagedByUserIdAsync(
                request.UserId,
                request.PageNumber,
                request.PageSize,
                request.WalletId,
                request.CategoryId,
                request.FromDate,
                request.ToDate,
                cancellationToken);

            var dtos = items.Select(t => new TransactionDto(
                t.Id,
                t.UserId,
                t.WalletId,
                t.CategoryId,
                t.Amount,
                (int)t.Type,
                t.Description,
                t.TransactionDate
                 )).ToList();

            var pagedResult = new PagedResult<TransactionDto>(dtos, totalCount, request.PageNumber, request.PageSize);

            return Result<PagedResult<TransactionDto>>.Success(pagedResult);
        }
    }
}
