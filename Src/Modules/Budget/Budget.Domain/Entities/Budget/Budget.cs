using Budget.Domain.Entities.Budget.Events;
using FinTracker.SharedKernel.Domain;
using FinTracker.SharedKernel.Exceptions;
using System;

namespace Budget.Domain.Entities.Budget
{
    public class Budget : AggregateRoot<Guid>
    {
        public Guid UserId { get; private set; }
        public int CategoryId { get; private set; }
        public decimal TargetAmount { get; private set; }
        public decimal CurrentSpentAmount { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Tracks whether warning threshold (e.g., 80%) was already raised
        public bool HasReachedWarningThreshold { get; private set; }
        // Tracks whether full budget was exceeded (100%+)
        public bool HasExceededBudget { get; private set; }

        protected Budget() : base(Guid.Empty)
        {
        }

        private Budget(
            Guid id,
            Guid userId,
            int categoryId,
            decimal targetAmount,
            int month,
            int year) : base(id)
        {
            if (userId == Guid.Empty)
                throw new DomainException("UserId cannot be empty.");

            if (categoryId <= 0)
                throw new DomainException("CategoryId must be a valid positive identifier.");

            if (targetAmount <= 0)
                throw new DomainException("TargetAmount must be greater than zero.");

            if (month < 1 || month > 12)
                throw new DomainException("Month must be between 1 and 12.");

            if (year < 2000)
                throw new DomainException("Invalid year.");

            UserId = userId;
            CategoryId = categoryId;
            TargetAmount = targetAmount;
            CurrentSpentAmount = 0;
            Month = month;
            Year = year;
            CreatedAt = DateTime.UtcNow;
            HasReachedWarningThreshold = false;
            HasExceededBudget = false;
        }

        public static Budget Create(
            Guid userId,
            int categoryId,
            decimal targetAmount,
            int month,
            int year)
        {
            var budget = new Budget(
                Guid.NewGuid(),
                userId,
                categoryId,
                targetAmount,
                month,
                year);

            budget.AddDomainEvent(new BudgetCreated(budget));

            return budget;
        }

        public void UpdateAmount(decimal newTargetAmount)
        {
            if (newTargetAmount <= 0)
                throw new DomainException("TargetAmount must be greater than zero.");

            if (TargetAmount == newTargetAmount)
                return;

            TargetAmount = newTargetAmount;
            UpdatedAt = DateTime.UtcNow;

            // Re-evaluate thresholds against current spendings
            EvaluateThresholds();

            AddDomainEvent(new BudgetAmountUpdated(this));
        }

        /// <summary>
        /// Registers newly spent amount or adjusts the total spent amount.
        /// </summary>
        public void RecordExpense(decimal amount)
        {
            if (amount <= 0)
                throw new DomainException("Expense amount must be greater than zero.");

            CurrentSpentAmount += amount;
            UpdatedAt = DateTime.UtcNow;

            EvaluateThresholds();
        }

        public void RevertExpense(decimal amount)
        {
            if (amount <= 0)
            {
                throw new DomainException("Amount to revert must be greater than zero.");
            }

            CurrentSpentAmount = Math.Max(0, CurrentSpentAmount - amount);
            EvaluateThresholds();
        }

        public decimal CalculateSpentPercentage()
        {
            if (TargetAmount <= 0) return 0;
            return Math.Round((CurrentSpentAmount / TargetAmount) * 100, 2);
        }

        private void EvaluateThresholds()
        {
            var percentage = CalculateSpentPercentage();

            // Threshold 1: 80% warning
            if (percentage >= 80)
            {
                if (!HasReachedWarningThreshold)
                {
                    HasReachedWarningThreshold = true;
                    AddDomainEvent(new FinTracker.SharedKernel.EventsContracts.BudgetWarningThresholdReachedEvent(
                        Id,
                        UserId,
                        CategoryId,
                        TargetAmount,
                        CurrentSpentAmount,
                        percentage));
                }
            }
            else
            {
                // Reset flag if spent amount drops below 80% (e.g. when an expense is deleted)
                HasReachedWarningThreshold = false;
            }

            // Threshold 2: 100% exceeded
            if (percentage >= 100)
            {
                if (!HasExceededBudget)
                {
                    HasExceededBudget = true;
                    AddDomainEvent(new FinTracker.SharedKernel.EventsContracts.BudgetExceededEvent(
                        Id,
                        UserId,
                        CategoryId,
                        TargetAmount,
                        CurrentSpentAmount,
                        percentage));
                }
            }
            else
            {
                // Reset flag if spent amount drops below 100%
                HasExceededBudget = false;
            }
        }

    }
}
