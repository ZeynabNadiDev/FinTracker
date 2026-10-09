using Budget.Domain.Repositories;
using Budget.Domain.UOW;
using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Application.Commands.UpdateBudgetAmount.Handler
{
    public class UpdateBudgetAmountCommandHandler : IRequestHandler<UpdateBudgetAmountCommand, Result>
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBudgetAmountCommandHandler(IBudgetRepository budgetRepository, IUnitOfWork unitOfWork)
        {
            _budgetRepository = budgetRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateBudgetAmountCommand request, CancellationToken cancellationToken)
        {
            var budget = await _budgetRepository.GetByIdAsync(request.BudgetId, cancellationToken);

            if (budget == null || budget.UserId != request.UserId)
            {
                return Result.Failure("Budget not found.");
            }

            budget.UpdateAmount(request.NewTargetAmount);

            _budgetRepository.Update(budget);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
