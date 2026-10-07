using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Application.DTOs;
using Transaction.Domain.Repositories;

namespace Transaction.Application.Queries.GetTransactionsByUserId.Handler
{
    public class GetTransactionsByUserIdQueryHandler : IRequestHandler<GetTransactionsByUserIdQuery, Result<IReadOnlyList<TransactionDto>>>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionsByUserIdQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Result<IReadOnlyList<TransactionDto>>> Handle(GetTransactionsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            var dtos = transactions
                .Where(t => !t.IsRemoved)
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new TransactionDto(
                    t.Id,
                    t.UserId,
                    t.WalletId,
                    t.CategoryId,
                    t.Amount,
                    (int)t.Type,
                    t.Description,
                    t.TransactionDate,
                    t.DestinationWalletId
                ))
                .ToList();

            return Result<IReadOnlyList<TransactionDto>>.Success(dtos);
        }
    }
}
