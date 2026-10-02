using FinTracker.SharedKernel.Contracts;
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
        private readonly IWalletContract _walletContract;
        private readonly ICategoryContract _categoryContract;

        public CreateTransactionCommandHandler(
            ITransactionRepository repository,
            IUnitOfWork unitOfWork,
            IWalletContract walletContract,
            ICategoryContract categoryContract)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _walletContract = walletContract;
            _categoryContract = categoryContract;
        }

        public async Task<Result<TransactionDto>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            // 1. Guard Clause: Wallet Ownership Check
            var isWalletValid = await _walletContract.IsWalletOwnedByUserAsync(request.WalletId, request.UserId, cancellationToken);
            if (!isWalletValid)
            {
                return Result<TransactionDto>.Failure("You do not have access to this wallet or it does not exist.");
            }

            // 2. Guard Clause: Category Ownership Check
            var isCategoryValid = await _categoryContract.IsCategoryAccessibleByUserAsync(request.CategoryId, request.UserId, cancellationToken);
            if (!isCategoryValid)
            {
                return Result<TransactionDto>.Failure("You do not have access to this category or it does not exist.");
            }

            // 3. Create transaction aggregate instance using system server time (DateTime.UtcNow)
            var transaction = Domain.Entities.Transaction.Transaction.Create(
                request.UserId,
                request.WalletId,
                request.CategoryId,
                request.Amount,
                request.Type,
                request.Description,
                DateTime.UtcNow);

            // 4. Add entity to repository
            await _repository.AddAsync(transaction, cancellationToken);

            // 5. Commit unit of work
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 6. Map to response DTO
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
