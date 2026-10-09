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
    public sealed class TransactionCreatedEventHandler : INotificationHandler<TransactionCreatedEvent>
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransactionCreatedEventHandler> _logger;

        public TransactionCreatedEventHandler(
            IBudgetRepository budgetRepository,
            IUnitOfWork unitOfWork,
            ILogger<TransactionCreatedEventHandler> logger)
        {
            _budgetRepository = budgetRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(TransactionCreatedEvent notification, CancellationToken cancellationToken)
        {
            // 1. We only track expenses that belong to a specific category
            if (notification.Type != TransactionType.Expense || !notification.CategoryId.HasValue)
            {
                return;
            }

            var transactionDate = notification.OccurredOn;
            var month = transactionDate.Month;
            var year = transactionDate.Year;

            // 2. Fetch the budget for the user, category, and current period
            var budget = await _budgetRepository.GetByUserCategoryAndPeriodAsync(
                notification.UserId,
                notification.CategoryId.Value,
                month,
                year,
                cancellationToken);

            // If no budget is defined for this category/period, there's nothing to update
            if (budget is null)
            {
                return;
            }

            // 3. Record the expense (this evaluates thresholds and triggers domain events if exceeded)
            budget.RecordExpense(notification.Amount);

            _budgetRepository.Update(budget);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Updated budget {BudgetId} for user {UserId}. Current spent: {CurrentSpentAmount}",
                budget.Id,
                notification.UserId,
                budget.CurrentSpentAmount);
        }
    }
}
