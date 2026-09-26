using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Application.DTOs;
using Transaction.Domain.Repositories;
using Transaction.Domain.UOW;

namespace Transaction.Application.Commands.CreateTransaction.Handler
{
    public sealed class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<TransactionDto>>
    {
        private readonly ITransactionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTransactionCommandHandler(ITransactionRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<TransactionDto>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            // Create transaction aggregate instance
            var transaction = Domain.Entities.Transaction.Transaction.Create(
                request.UserId,
                request.WalletId,
                request.CategoryId,
                request.Amount,
                request.Type,
                request.Description,
                request.TransactionDate);

            // Add entity to repository
            await _repository.AddAsync(transaction, cancellationToken);

            // Commit unit of work
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Map to response DTO
            var transactionDto = new TransactionDto(
                transaction.Id,
                transaction.UserId,
                transaction.WalletId,
                transaction.CategoryId,
                transaction.Amount,
                (int)transaction.Type,
                transaction.Description,
                transaction.TransactionDate);

            return Result<TransactionDto>.Success(transactionDto);
        }
    }
}
