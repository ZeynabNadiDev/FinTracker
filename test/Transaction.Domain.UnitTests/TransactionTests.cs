using FinTracker.SharedKernel.Enums;
using FinTracker.SharedKernel.Events;
using FinTracker.SharedKernel.Exceptions;
using FluentAssertions;
using Transaction.Domain.Entities.Transaction;
using Xunit;

namespace Transaction.Domain.UnitTests
{
    public class TransactionTests
    {
        private readonly Guid _validUserId = Guid.NewGuid();
        private readonly Guid _validWalletId = Guid.NewGuid();
        private readonly Guid _validDestinationWalletId = Guid.NewGuid();
        private const int ValidCategoryId = 10;
        private const decimal ValidAmount = 1500.50m;

        #region Create (Income / Expense) Tests

        [Theory]
        [InlineData(TransactionType.Income)]
        [InlineData(TransactionType.Expense)]
        public void Create_WithValidParameters_ShouldInstantiateTransactionAndRaiseEvent(TransactionType type)
        {
            // Arrange
            var description = "  Monthly payment  ";
            var transactionDate = DateTime.UtcNow.AddHours(-1);

            // Act
            var transaction = Transaction.Domain.Entities.Transaction.Transaction.Create(
                _validUserId,
                _validWalletId,
                ValidCategoryId,
                ValidAmount,
                type,
                description,
                transactionDate);

            // Assert
            transaction.Should().NotBeNull();
            transaction.Id.Should().NotBeEmpty();
            transaction.UserId.Should().Be(_validUserId);
            transaction.WalletId.Should().Be(_validWalletId);
            transaction.DestinationWalletId.Should().BeNull();
            transaction.CategoryId.Should().Be(ValidCategoryId);
            transaction.Amount.Should().Be(ValidAmount);
            transaction.Type.Should().Be(type);
            transaction.Description.Should().Be("Monthly payment");
            transaction.TransactionDate.Should().Be(transactionDate);
            transaction.IsRemoved.Should().BeFalse();
            transaction.UpdatedAt.Should().BeNull();
            transaction.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

            // Verify Domain Event
            transaction.DomainEvents.Should().ContainSingle(e => e is TransactionCreatedEvent);
        }

        [Fact]
        public void Create_WhenTypeIsTransfer_ShouldThrowDomainException()
        {
            // Act
            var act = () => Transaction.Domain.Entities.Transaction.Transaction.Create(
                _validUserId,
                _validWalletId,
                ValidCategoryId,
                ValidAmount,
                TransactionType.Transfer,
                "Transfer test",
                DateTime.UtcNow);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Use CreateTransfer method for transfer transactions.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-5)]
        public void Create_WithInvalidCategoryId_ShouldThrowDomainException(int? categoryId)
        {
            // Act
            var act = () => Transaction.Domain.Entities.Transaction.Transaction.Create(
                _validUserId,
                _validWalletId,
                categoryId.GetValueOrDefault(),
                ValidAmount,
                TransactionType.Expense,
                "Expense test",
                DateTime.UtcNow);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("CategoryId must be a valid positive identifier for non-transfer transactions.");
        }

        #endregion

        #region CreateTransfer Tests

        [Fact]
        public void CreateTransfer_WithValidParameters_ShouldInstantiateTransactionAndRaiseEvent()
        {
            // Arrange
            var description = "Wallet transfer";
            var transactionDate = DateTime.UtcNow;

            // Act
            var transaction = Transaction.Domain.Entities.Transaction.Transaction.CreateTransfer(
                _validUserId,
                _validWalletId,
                _validDestinationWalletId,
                ValidAmount,
                description,
                transactionDate);

            // Assert
            transaction.Should().NotBeNull();
            transaction.Id.Should().NotBeEmpty();
            transaction.UserId.Should().Be(_validUserId);
            transaction.WalletId.Should().Be(_validWalletId);
            transaction.DestinationWalletId.Should().Be(_validDestinationWalletId);
            transaction.CategoryId.Should().BeNull();
            transaction.Amount.Should().Be(ValidAmount);
            transaction.Type.Should().Be(TransactionType.Transfer);
            transaction.Description.Should().Be(description);
            transaction.TransactionDate.Should().Be(transactionDate);
            transaction.IsRemoved.Should().BeFalse();

            // Verify Domain Event
            transaction.DomainEvents.Should().ContainSingle(e => e is TransactionCreatedEvent);
        }

        [Fact]
        public void CreateTransfer_WithoutDescription_ShouldSetDefaultDescription()
        {
            // Act
            var transaction = Transaction.Domain.Entities.Transaction.Transaction.CreateTransfer(
                _validUserId,
                _validWalletId,
                _validDestinationWalletId,
                ValidAmount);

            // Assert
            transaction.Description.Should().Be($"Transfer to wallet {_validDestinationWalletId}");
        }

        [Fact]
        public void CreateTransfer_WhenDestinationWalletIsSameAsSourceWallet_ShouldThrowDomainException()
        {
            // Act
            var act = () => Transaction.Domain.Entities.Transaction.Transaction.CreateTransfer(
                _validUserId,
                _validWalletId,
                _validWalletId, // Same as source
                ValidAmount);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Destination wallet cannot be the same as source wallet.");
        }

        [Fact]
        public void CreateTransfer_WhenDestinationWalletIsEmptyGuid_ShouldThrowDomainException()
        {
            // Act
            var act = () => Transaction.Domain.Entities.Transaction.Transaction.CreateTransfer(
                _validUserId,
                _validWalletId,
                Guid.Empty,
                ValidAmount);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Destination wallet is required for transfer transactions.");
        }

        #endregion

        #region Common Validation Tests

        [Fact]
        public void Create_WhenUserIdIsEmpty_ShouldThrowDomainException()
        {
            // Act
            var act = () => Transaction.Domain.Entities.Transaction.Transaction.Create(
                Guid.Empty,
                _validWalletId,
                ValidCategoryId,
                ValidAmount,
                TransactionType.Income,
                null,
                DateTime.UtcNow);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("UserId cannot be empty.");
        }

        [Fact]
        public void Create_WhenWalletIdIsEmpty_ShouldThrowDomainException()
        {
            // Act
            var act = () => Transaction.Domain.Entities.Transaction.Transaction.Create(
                _validUserId,
                Guid.Empty,
                ValidCategoryId,
                ValidAmount,
                TransactionType.Income,
                null,
                DateTime.UtcNow);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("WalletId cannot be empty.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Create_WhenAmountIsZeroOrNegative_ShouldThrowDomainException(decimal invalidAmount)
        {
            // Act
            var act = () => Transaction.Domain.Entities.Transaction.Transaction.Create(
                _validUserId,
                _validWalletId,
                ValidCategoryId,
                invalidAmount,
                TransactionType.Income,
                null,
                DateTime.UtcNow);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Amount must be greater than zero.");
        }

        #endregion

        #region Remove Tests

        [Fact]
        public void Remove_WhenActive_ShouldMarkAsRemovedAndRaiseEvent()
        {
            // Arrange
            var transaction = Transaction.Domain.Entities.Transaction.Transaction.Create(
                _validUserId,
                _validWalletId,
                ValidCategoryId,
                ValidAmount,
                TransactionType.Expense,
                "Test",
                DateTime.UtcNow);
            transaction.ClearDomainEvents();

            // Act
            transaction.Remove();

            // Assert
            transaction.IsRemoved.Should().BeTrue();
            transaction.UpdatedAt.Should().NotBeNull();
            transaction.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

            // Verify Domain Event
            transaction.DomainEvents.Should().ContainSingle(e => e is TransactionDeletedEvent);
        }

        [Fact]
        public void Remove_WhenAlreadyRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var transaction = Transaction.Domain.Entities.Transaction.Transaction.Create(
                _validUserId,
                _validWalletId,
                ValidCategoryId,
                ValidAmount,
                TransactionType.Expense,
                "Test",
                DateTime.UtcNow);
            transaction.Remove();

            // Act
            var act = () => transaction.Remove();

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Transaction is already removed.");
        }

        #endregion
    }
}
