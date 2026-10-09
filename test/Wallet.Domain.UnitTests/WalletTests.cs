using FinTracker.SharedKernel.Events;
using FinTracker.SharedKernel.Exceptions;
using FluentAssertions;
using Wallet.Domain.Entities.Wallet;
using Wallet.Domain.Entities.Wallet.Events;
using Xunit;

namespace Wallet.Domain.UnitTests
{
    public class WalletTests
    {
        private readonly Guid _validUserId = Guid.NewGuid();

        #region Create & Update & Remove Tests

        [Fact]
        public void Create_WithValidParameters_ShouldInstantiateWalletAndRaiseEvent()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var title = "  Main Wallet  ";
            var currency = "IRT";

            // Act
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(walletId, title, _validUserId, currency);

            // Assert
            wallet.Should().NotBeNull();
            wallet.Id.Should().Be(walletId);
            wallet.Title.Should().Be("Main Wallet");
            wallet.UserId.Should().Be(_validUserId);
            wallet.Balance.Amount.Should().Be(0);
            wallet.Balance.Currency.Should().Be(currency);
            wallet.IsRemoved.Should().BeFalse();
            wallet.UpdatedAt.Should().BeNull();
            wallet.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

            // Verify Domain Event
            wallet.DomainEvents.Should().ContainSingle(e => e is WalletCreated);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithEmptyTitle_ShouldThrowDomainException(string? invalidTitle)
        {
            // Act
            var act = () => Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), invalidTitle!, _validUserId);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("title cannot be null or empty.");
        }

        [Fact]
        public void Update_WithValidTitle_ShouldUpdateTitleAndRaiseEvent()
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Old Title", _validUserId);
            wallet.ClearDomainEvents();

            // Act
            wallet.Update("  New Title  ");

            // Assert
            wallet.Title.Should().Be("New Title");
            wallet.UpdatedAt.Should().NotBeNull();
            wallet.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

            // Verify Domain Event
            wallet.DomainEvents.Should().ContainSingle(e => e is WalletUpdated);
        }

        [Fact]
        public void Update_WhenWalletIsRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Title", _validUserId);
            wallet.Remove();

            // Act
            var act = () => wallet.Update("Updated Title");

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Removed wallet cannot be updated.");
        }

        [Fact]
        public void Remove_WhenActive_ShouldMarkAsRemovedAndRaiseEvent()
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Salary Wallet", _validUserId);
            wallet.ClearDomainEvents();

            // Act
            wallet.Remove();

            // Assert
            wallet.IsRemoved.Should().BeTrue();
            wallet.UpdatedAt.Should().NotBeNull();
            wallet.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

            // Verify Domain Event
            wallet.DomainEvents.Should().ContainSingle(e => e is WalletDeleted);
        }

        [Fact]
        public void Remove_WhenAlreadyRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Salary Wallet", _validUserId);
            wallet.Remove();

            // Act
            var act = () => wallet.Remove();

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Wallet is already removed.");
        }

        #endregion

        #region Deposit & Withdraw Tests

        [Fact]
        public void Deposit_WithValidAmount_ShouldIncreaseBalance()
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Wallet", _validUserId);

            // Act
            wallet.Deposit(500000m);

            // Assert
            wallet.Balance.Amount.Should().Be(500000m);
            wallet.UpdatedAt.Should().NotBeNull();
            wallet.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1000)]
        public void Deposit_WithZeroOrNegativeAmount_ShouldThrowDomainException(decimal invalidAmount)
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Wallet", _validUserId);

            // Act
            var act = () => wallet.Deposit(invalidAmount);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Deposit amount must be greater than zero.");
        }

        [Fact]
        public void Deposit_WhenWalletIsRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Wallet", _validUserId);
            wallet.Remove();

            // Act
            var act = () => wallet.Deposit(100000m);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Cannot deposit to a removed wallet.");
        }

        [Fact]
        public void Withdraw_WithSufficientBalance_ShouldDecreaseBalance()
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Wallet", _validUserId);
            wallet.Deposit(1000000m);

            // Act
            wallet.Withdraw(300000m);

            // Assert
            wallet.Balance.Amount.Should().Be(700000m);
            wallet.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public void Withdraw_WithInsufficientBalance_ShouldThrowDomainException()
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Wallet", _validUserId);
            wallet.Deposit(200000m);

            // Act
            var act = () => wallet.Withdraw(500000m);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Insufficient wallet balance.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-500)]
        public void Withdraw_WithZeroOrNegativeAmount_ShouldThrowDomainException(decimal invalidAmount)
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Wallet", _validUserId);

            // Act
            var act = () => wallet.Withdraw(invalidAmount);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Withdraw amount must be greater than zero.");
        }

        [Fact]
        public void Withdraw_WhenWalletIsRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Wallet", _validUserId);
            wallet.Deposit(500000m);
            wallet.Remove();

            // Act
            var act = () => wallet.Withdraw(100000m);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Cannot withdraw from a removed wallet.");
        }

        #endregion

        #region Transfer Tests

        [Fact]
        public void TransferTo_WithValidParameters_ShouldTransferFundsAndRaiseEvent()
        {
            // Arrange
            var sourceWallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Source Wallet", _validUserId, "IRT");
            var destinationWallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Destination Wallet", _validUserId, "IRT");

            sourceWallet.Deposit(1000000m);
            sourceWallet.ClearDomainEvents();

            var transferAmount = 400000m;

            // Act
            sourceWallet.TransferTo(destinationWallet, transferAmount);

            // Assert
            sourceWallet.Balance.Amount.Should().Be(600000m);
            destinationWallet.Balance.Amount.Should().Be(400000m);
            sourceWallet.UpdatedAt.Should().NotBeNull();
            destinationWallet.UpdatedAt.Should().NotBeNull();

            // Verify Domain Event
            sourceWallet.DomainEvents.Should().ContainSingle(e => e is WalletTransferredEvent);
        }

        [Fact]
        public void TransferTo_ToSameWallet_ShouldThrowDomainException()
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Wallet", _validUserId);
            wallet.Deposit(500000m);

            // Act
            var act = () => wallet.TransferTo(wallet, 100000m);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Cannot transfer money to the same wallet.");
        }

        [Fact]
        public void TransferTo_WhenDestinationWalletIsNull_ShouldThrowDomainException()
        {
            // Arrange
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Wallet", _validUserId);
            wallet.Deposit(500000m);

            // Act
            var act = () => wallet.TransferTo(null!, 100000m);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Destination wallet cannot be null.");
        }

        [Fact]
        public void TransferTo_WhenSourceWalletIsRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var sourceWallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Source", _validUserId);
            var destinationWallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Destination", _validUserId);
            sourceWallet.Deposit(500000m);
            sourceWallet.Remove();

            // Act
            var act = () => sourceWallet.TransferTo(destinationWallet, 100000m);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Cannot transfer from a removed wallet.");
        }

        [Fact]
        public void TransferTo_WhenDestinationWalletIsRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var sourceWallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Source", _validUserId);
            var destinationWallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Destination", _validUserId);
            sourceWallet.Deposit(500000m);
            destinationWallet.Remove();

            // Act
            var act = () => sourceWallet.TransferTo(destinationWallet, 100000m);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Cannot transfer to a removed wallet.");
        }

        [Fact]
        public void TransferTo_WithMismatchedCurrencies_ShouldThrowDomainException()
        {
            // Arrange
            var sourceWallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "IRT Wallet", _validUserId, "IRT");
            var destinationWallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "USD Wallet", _validUserId, "USD");
            sourceWallet.Deposit(500000m);

            // Act
            var act = () => sourceWallet.TransferTo(destinationWallet, 100000m);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Currency mismatch: cannot transfer from IRT to USD.");
        }

        [Fact]
        public void TransferTo_WithInsufficientBalance_ShouldThrowDomainException()
        {
            // Arrange
            var sourceWallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Source", _validUserId, "IRT");
            var destinationWallet = Wallet.Domain.Entities.Wallet.Wallet.Create(Guid.NewGuid(), "Destination", _validUserId, "IRT");
            sourceWallet.Deposit(100000m);

            // Act
            var act = () => sourceWallet.TransferTo(destinationWallet, 500000m);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Insufficient wallet balance.");
        }

        #endregion
    }
}
