using Category.Domain.Entities.Category;
using Category.Domain.Entities.Category.Events;
using Category.Domain.Enums;
using FinTracker.SharedKernel.Exceptions;
using FluentAssertions;
using Xunit;

namespace Category.Domain.UnitTests
{
    public class CategoryTests
    {
        [Fact]
        public void Create_WithValidParameters_ShouldInstantiateCategoryAndRaiseEvent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var name = "  Groceries  ";
            var description = "  Monthly grocery expenses  ";
            var type = CategoryType.Expense;

            // Act
            var category = Entities.Category.Category.Create(name, description, userId, type);

            // Assert
            category.Should().NotBeNull();
            category.Name.Should().Be("Groceries"); // Trim check
            category.Description.Should().Be("Monthly grocery expenses"); // Trim check
            category.UserId.Should().Be(userId);
            category.Type.Should().Be(type);
            category.IsRemoved.Should().BeFalse();
            category.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
            category.UpdatedAt.Should().BeNull();

            // Verify Domain Event
            category.DomainEvents.Should().ContainSingle(e => e is CategoryCreated);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithNullOrWhiteSpaceName_ShouldThrowDomainException(string? invalidName)
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var act = () => Entities.Category.Category.Create(invalidName!, "Valid description", userId, CategoryType.Expense);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("name cannot be null or empty.");
        }

        [Fact]
        public void Update_WithValidParameters_ShouldUpdateFieldsAndRaiseEvent()
        {
            // Arrange
            var category = Entities.Category.Category.Create("Salary", null, Guid.NewGuid(), CategoryType.Income);
            category.ClearDomainEvents();

            var newName = "  Bonus Salary  ";
            var newDescription = "  Annual bonus  ";
            var newType = CategoryType.Income;

            // Act
            category.Update(newName, newDescription, newType);

            // Assert
            category.Name.Should().Be("Bonus Salary");
            category.Description.Should().Be("Annual bonus");
            category.Type.Should().Be(newType);
            category.UpdatedAt.Should().NotBeNull();
            category.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

            // Verify Domain Event
            category.DomainEvents.Should().ContainSingle(e => e is CategoryUpdated);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_WithNullOrWhiteSpaceName_ShouldThrowDomainException(string? invalidName)
        {
            // Arrange
            var category = Entities.Category.Category.Create("Food", "Food expenses", Guid.NewGuid(), CategoryType.Expense);

            // Act
            var act = () => category.Update(invalidName!, "Updated description", CategoryType.Expense);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("name cannot be null or empty.");
        }

        [Fact]
        public void Update_WhenCategoryIsRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var category = Entities.Category.Category.Create("Subscriptions", "Netflix etc.", Guid.NewGuid(), CategoryType.Expense);
            category.Remove();

            // Act
            var act = () => category.Update("Active Subscriptions", "Updated", CategoryType.Expense);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Removed category cannot be updated.");
        }

        [Fact]
        public void Remove_WhenCategoryIsActive_ShouldMarkAsRemovedAndRaiseEvent()
        {
            // Arrange
            var category = Entities.Category.Category.Create("Gym", "Monthly membership", Guid.NewGuid(), CategoryType.Expense);
            category.ClearDomainEvents();

            // Act
            category.Remove();

            // Assert
            category.IsRemoved.Should().BeTrue();
            category.UpdatedAt.Should().NotBeNull();
            category.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

            // Verify Domain Event
            category.DomainEvents.Should().ContainSingle(e => e is CategoryDeleted);
        }

        [Fact]
        public void Remove_WhenCategoryIsAlreadyRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var category = Entities.Category.Category.Create("Gym", "Monthly membership", Guid.NewGuid(), CategoryType.Expense);
            category.Remove();

            // Act
            var act = () => category.Remove();

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Category is already removed.");
        }
    }
}
