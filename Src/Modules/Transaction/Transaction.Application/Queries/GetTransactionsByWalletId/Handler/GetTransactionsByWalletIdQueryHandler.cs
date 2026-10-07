using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Application.DTOs;
using Transaction.Domain.Repositories;

namespace Transaction.Application.Queries.GetTransactionsByWalletId.Handler
{
    public class GetTransactionsByWalletIdQueryHandler : IRequestHandler<GetTransactionsByWalletIdQuery, Result<IReadOnlyList<TransactionDto>>>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionsByWalletIdQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Result<IReadOnlyList<TransactionDto>>> Handle(GetTransactionsByWalletIdQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetByWalletIdAsync(request.WalletId, cancellationToken);

            // Filter out removed ones and ensure user security
            var dtos = transactions
                .Where(t => !t.IsRemoved && t.UserId == request.UserId)
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
