using Budget.Domain.Entities.Budget;
using Budget.Domain.Repositories;
using Budget.Domain.UOW;
using Budget.Infrastructure.Persistence.Repositories;
using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Application.Commands.CreateBudget.Handler
{
    public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Result<Guid>>
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBudgetCommandHandler(IBudgetRepository budgetRepository, IUnitOfWork unitOfWork)
        {
            _budgetRepository = budgetRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
        {
            var existingBudget = await _budgetRepository.GetByUserCategoryAndPeriodAsync(
                request.UserId,
                request.CategoryId,
                request.Month,
                request.Year,
                cancellationToken);

            if (existingBudget != null)
            {
                return Result<Guid>.Failure("Budget already exists for this category in the specified period.");
            }

            var budget = Budget.Domain.Entities.Budget.Budget.Create(
                request.UserId,
                request.CategoryId,
                request.TargetAmount,
                request.Month,
                request.Year);

            await _budgetRepository.AddAsync(budget, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(budget.Id);
        }
    }
}
