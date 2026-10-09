using FinTracker.SharedKernel.EventsContracts;
using FinTracker.SharedKernel.Exceptions;
using FluentAssertions;

namespace Budget.Domain.UnitTests
{
    public class BudgetTests
    {
        private static Budget.Domain.Entities.Budget.Budget CreateValidBudget(decimal targetAmount = 1000m)
        {
            return Budget.Domain.Entities.Budget.Budget.Create(
                userId: Guid.NewGuid(),
                categoryId: 1,
                targetAmount: targetAmount,
                month: 10,
                year: 2026);
        }
        public void RecordExpense_WithValidAmount_ShouldIncreaseCurrentSpentAmount()
        {
            // Arrange (Given)
            var budget = CreateValidBudget(targetAmount: 1000m);

            // Act (When)
            budget.RecordExpense(250m);

            // Assert (Then)
            budget.CurrentSpentAmount.Should().Be(250m);
        }

        [Fact]
        public void RecordExpense_WithZeroOrNegativeAmount_ShouldThrowDomainException()
        {
            // Arrange
            var budget = CreateValidBudget();

            // Act
            Action act = () => budget.RecordExpense(-50m);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Expense amount must be greater than zero.");
        }

        [Fact]
        public void RecordExpense_WhenReachingEightyPercent_ShouldRaiseWarningEvent()
        {
            // Arrange: Budget with target 1000
            var budget = CreateValidBudget(targetAmount: 1000m);

            // Act: Spend 800 (which is exactly 80%)
            budget.RecordExpense(800m);

            // Assert
            budget.HasReachedWarningThreshold.Should().BeTrue();
            budget.DomainEvents.Should().ContainSingle(e => e is BudgetWarningThresholdReachedEvent);
        }

        [Fact]
        public void RecordExpense_WhenReachingTargetAmount_ShouldRaiseExceededEvent()
        {
            // Arrange: Budget with target 1000
            var budget = CreateValidBudget(targetAmount: 1000m);

            // Act: Spend 1000 (100%)
            budget.RecordExpense(1000m);

            // Assert
            budget.HasExceededBudget.Should().BeTrue();
            budget.DomainEvents.Should().Contain(e => e is BudgetExceededEvent);
        }
        [Fact]
        public void RevertExpense_WhenAmountIsValid_ShouldDecreaseCurrentSpentAmountAndResetFlags()
        {
            // Arrange
            var budget = Budget.Domain.Entities.Budget.Budget.Create(Guid.NewGuid(), 1, 1000m, 10, 2026);
            budget.RecordExpense(850m); // 85% spent -> Warning triggered
            budget.HasReachedWarningThreshold.Should().BeTrue();

            // Act
            budget.RevertExpense(100m); // Drops to 750m (75%)

            // Assert
            budget.CurrentSpentAmount.Should().Be(750m);
            budget.HasReachedWarningThreshold.Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public void RevertExpense_WithZeroOrNegativeAmount_ShouldThrowDomainException(decimal invalidAmount)
        {
            // Arrange
            var budget = Budget.Domain.Entities.Budget.Budget.Create(Guid.NewGuid(), 1, 1000m, 10, 2026);

            // Act
            Action act = () => budget.RevertExpense(invalidAmount);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Amount to revert must be greater than zero.");
        }

    }
}