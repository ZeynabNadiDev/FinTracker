using FinTracker.SharedKernel.Results;
using MediatR;
using Transaction.Domain.Repositories;
using Transaction.Domain.UOW;


namespace Transaction.Application.Commands.DeleteTransaction.Handler
{
    public sealed class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Result>
    {
        private readonly ITransactionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTransactionCommandHandler(ITransactionRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            // Retrieve transaction by id
            var transaction = await _repository.GetByIdAsync(request.TransactionId, cancellationToken);

            if (transaction is null)
            {
                return Result.Failure("Transaction was not found.");
            }

            // Security check: ensure the transaction belongs to the requesting user
            if (transaction.UserId != request.UserId)
            {
                return Result.Failure("You do not have permission to delete this transaction.");
            }

            // Execute domain logic for deletion (triggers soft delete & raises domain event)
            transaction.Remove();

            _repository.Update(transaction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
