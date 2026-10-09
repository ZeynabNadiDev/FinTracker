using FinTracker.SharedKernel.Exceptions;
using FluentAssertions;
using Notification.Domain.Enums;
using Xunit;

namespace Notification.Domain.UnitTests
{
    public class NotificationTests
    {
        [Fact]
        public void Create_WithValidParameters_ShouldCreateNotificationSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var title = " Budget Warning ";
            var message = " You have reached 80% of your budget. ";
            var type = NotificationType.BudgetWarning;

            // Act
            var notification = Entities.Notification.Notification.Create(id, userId, title, message, type);

            // Assert
            notification.Should().NotBeNull();
            notification.Id.Should().Be(id);
            notification.UserId.Should().Be(userId);
            notification.Title.Should().Be("Budget Warning");
            notification.Message.Should().Be("You have reached 80% of your budget.");
            notification.Type.Should().Be(type);
            notification.IsRead.Should().BeFalse();
            notification.ReadAt.Should().BeNull();
            notification.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public void MarkAsRead_WhenUnread_ShouldSetIsReadToTrueAndAssignReadAt()
        {
            // Arrange
            var notification = Entities.Notification.Notification.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Budget Exceeded",
                "You have exceeded your monthly budget limit.",
                NotificationType.BudgetExceeded);

            // Act
            notification.MarkAsRead();

            // Assert
            notification.IsRead.Should().BeTrue();
            notification.ReadAt.Should().NotBeNull();
            notification.ReadAt.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public async Task MarkAsRead_WhenAlreadyRead_ShouldNotUpdateReadAt()
        {
            // Arrange
            var notification = Entities.Notification.Notification.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Transfer Successful",
                "Your wallet transfer was completed successfully.",
                NotificationType.WalletTransfer);

            notification.MarkAsRead();
            var initialReadAt = notification.ReadAt;

            // Act: Wait briefly and call again to verify idempotency
            await Task.Delay(50);
            notification.MarkAsRead();

            // Assert
            notification.IsRead.Should().BeTrue();
            notification.ReadAt.Should().Be(initialReadAt);
        }

        [Fact]
        public void Create_WithEmptyUserId_ShouldThrowDomainException()
        {
            // Arrange
            var action = () => Entities.Notification.Notification.Create(
                Guid.NewGuid(),
                Guid.Empty,
                "General Notification",
                "This is a general system notification.",
                NotificationType.General);

            // Act & Assert
            action.Should().Throw<DomainException>()
                .WithMessage("UserId cannot be empty.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidTitle_ShouldThrowDomainException(string invalidTitle)
        {
            // Arrange
            var action = () => Entities.Notification.Notification.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                invalidTitle,
                "Valid message content.",
                NotificationType.General);

            // Act & Assert
            action.Should().Throw<DomainException>()
                .WithMessage("title cannot be null or empty.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidMessage_ShouldThrowDomainException(string invalidMessage)
        {
            // Arrange
            var action = () => Entities.Notification.Notification.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Valid Title",
                invalidMessage,
                NotificationType.General);

            // Act & Assert
            action.Should().Throw<DomainException>()
                .WithMessage("message cannot be null or empty.");
        }
    }
}
