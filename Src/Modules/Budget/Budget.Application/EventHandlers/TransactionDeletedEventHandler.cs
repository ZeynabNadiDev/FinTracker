using Budget.Domain.Repositories;
using Budget.Domain.UOW;
using FinTracker.SharedKernel.Enums;
using FinTracker.SharedKernel.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Application.EventHandlers
{
    public sealed class TransactionDeletedEventHandler : INotificationHandler<TransactionDeletedEvent>
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransactionDeletedEventHandler> _logger;

        public TransactionDeletedEventHandler(
            IBudgetRepository budgetRepository,
            IUnitOfWork unitOfWork,
            ILogger<TransactionDeletedEventHandler> logger)
        {
            _budgetRepository = budgetRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(TransactionDeletedEvent notification, CancellationToken cancellationToken)
        {
            // 1. Only process deleted expense transactions that had a category
            if (notification.Type != TransactionType.Expense || !notification.CategoryId.HasValue)
            {
                return;
            }

            var date = notification.OccurredOn;
            var month = date.Month;
            var year = date.Year;

            // 2. Find the relevant budget
            var budget = await _budgetRepository.GetByUserCategoryAndPeriodAsync(
                notification.UserId,
                notification.CategoryId.Value,
                month,
                year,
                cancellationToken);

            if (budget is null)
            {
                return;
            }

            // 3. Revert the spending
            budget.RevertExpense(notification.Amount);

            _budgetRepository.Update(budget);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Reverted expense of {Amount} on budget {BudgetId}. New spent amount: {CurrentSpentAmount}",
                notification.Amount,
                budget.Id,
                budget.CurrentSpentAmount);
        }
    }
}
