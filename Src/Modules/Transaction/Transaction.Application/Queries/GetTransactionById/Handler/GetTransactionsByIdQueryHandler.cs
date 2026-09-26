using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Application.DTOs;
using Transaction.Domain.Repositories;

namespace Transaction.Application.Queries.GetTransactionById.Handler
{
    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, Result<TransactionDto>>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Result<TransactionDto>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);

            if (transaction is null || transaction.IsRemoved)
            {
                return Result<TransactionDto>.Failure("Transaction not found.");
            }

            if (transaction.UserId != request.UserId)
            {
                return Result<TransactionDto>.Failure("You do not have permission to view this transaction.");
            }

            var dto = new TransactionDto(
                transaction.Id,
                transaction.UserId,
                transaction.WalletId,
                transaction.CategoryId,
                transaction.Amount,
                (int)transaction.Type,
                transaction.Description,
                transaction.TransactionDate
            );

            return Result<TransactionDto>.Success(dto);
        }
    }
}