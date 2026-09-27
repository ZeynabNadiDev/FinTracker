using Budget.Domain.Entities.Budget.Events;
using FinTracker.SharedKernel.Domain;
using FinTracker.SharedKernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Domain.Entities.Budget
{
    public class Budget: AggregateRoot<Guid>
    {
        public Guid UserId { get; private set; }
        public int CategoryId { get; private set; }
        public decimal TargetAmount { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

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
            Month = month;
            Year = year;
            CreatedAt = DateTime.UtcNow;
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

            var oldAmount = TargetAmount;
            TargetAmount = newTargetAmount;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new BudgetAmountUpdated(this));
        }
    }
 }
