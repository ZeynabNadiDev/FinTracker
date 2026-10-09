using FinTracker.SharedKernel.Exceptions;
using FluentAssertions;
using Identity.Domain.Entities.User;
using Identity.Domain.Entities.User.Events;
using Identity.Domain.ValueObject;
using Xunit;

namespace Identity.Domain.UnitTests
{
    public class UserTests
    {
        private readonly Email _validEmail = new("zeinab@example.com");

        [Fact]
        public void Create_WithValidParameters_ShouldInstantiateUserAndRaiseEvent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var phone = "  +989123456789  ";
            var firstName = "  Zeinab  ";
            var lastName = "  Dev  ";
            var passwordHash = "hashed_secret_password";

            // Act
            var user = User.Create(userId, _validEmail, phone, firstName, lastName, passwordHash);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(userId);
            user.Email.Should().Be(_validEmail);
            user.PhoneNumber.Should().Be("+989123456789");
            user.FirstName.Should().Be("Zeinab");
            user.LastName.Should().Be("Dev");
            user.PasswordHash.Should().Be(passwordHash);
            user.IsRemoved.Should().BeFalse();
            user.LastLogin.Should().BeNull();
            user.UpdatedAt.Should().BeNull();
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

            // Verify Domain Event
            user.DomainEvents.Should().ContainSingle(e => e is UserCreated);
        }

        [Theory]
        [InlineData(null, "Zeinab", "Dev", "hashed")]
        [InlineData("   ", "Zeinab", "Dev", "hashed")]
        [InlineData("+989123456789", null, "Dev", "hashed")]
        [InlineData("+989123456789", "   ", "Dev", "hashed")]
        [InlineData("+989123456789", "Zeinab", null, "hashed")]
        [InlineData("+989123456789", "Zeinab", "   ", "hashed")]
        [InlineData("+989123456789", "Zeinab", "Dev", null)]
        [InlineData("+989123456789", "Zeinab", "Dev", "   ")]
        public void Create_WithEmptyFields_ShouldThrowDomainException(
            string? phone,
            string? firstName,
            string? lastName,
            string? passwordHash)
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var act = () => User.Create(userId, _validEmail, phone!, firstName!, lastName!, passwordHash!);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("*cannot be null or empty.");
        }

        [Fact]
        public void Update_WithValidParameters_ShouldUpdateFieldsAndRaiseEvent()
        {
            // Arrange
            var user = User.Create(Guid.NewGuid(), _validEmail, "09120000000", "OldName", "OldFamily", "hashed");
            user.ClearDomainEvents();

            var newEmail = new Email("newemail@example.com");

            // Act
            user.Update(newEmail, "  09121111111  ", "  NewName  ", "  NewFamily  ");

            // Assert
            user.Email.Should().Be(newEmail);
            user.PhoneNumber.Should().Be("09121111111");
            user.FirstName.Should().Be("NewName");
            user.LastName.Should().Be("NewFamily");
            user.UpdatedAt.Should().NotBeNull();
            user.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

            // Verify Domain Event
            user.DomainEvents.Should().ContainSingle(e => e is UserUpdated);
        }

        [Fact]
        public void Update_WhenUserIsRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var user = User.Create(Guid.NewGuid(), _validEmail, "09120000000", "Zeinab", "Dev", "hashed");
            user.Remove();

            // Act
            var act = () => user.Update(_validEmail, "09120000000", "Updated", "Dev");

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Removed user cannot be updated.");
        }

        [Fact]
        public void Remove_WhenUserIsActive_ShouldMarkAsRemovedAndRaiseEvent()
        {
            // Arrange
            var user = User.Create(Guid.NewGuid(), _validEmail, "09120000000", "Zeinab", "Dev", "hashed");
            user.ClearDomainEvents();

            // Act
            user.Remove();

            // Assert
            user.IsRemoved.Should().BeTrue();
            user.UpdatedAt.Should().NotBeNull();
            user.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

            // Verify Domain Event
            user.DomainEvents.Should().ContainSingle(e => e is UserDeleted);
        }

        [Fact]
        public void Remove_WhenUserIsAlreadyRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var user = User.Create(Guid.NewGuid(), _validEmail, "09120000000", "Zeinab", "Dev", "hashed");
            user.Remove();

            // Act
            var act = () => user.Remove();

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("User is already removed.");
        }

        [Fact]
        public void ChangePassword_WithValidPassword_ShouldUpdatePasswordHashAndRaiseEvent()
        {
            // Arrange
            var user = User.Create(Guid.NewGuid(), _validEmail, "09120000000", "Zeinab", "Dev", "old_hash");
            user.ClearDomainEvents();

            // Act
            user.ChangePassword("new_hashed_password");

            // Assert
            user.PasswordHash.Should().Be("new_hashed_password");
            user.UpdatedAt.Should().NotBeNull();
            user.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

            // Verify Domain Event
            user.DomainEvents.Should().ContainSingle(e => e is UserPasswordChanged);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ChangePassword_WithNullOrWhiteSpace_ShouldThrowDomainException(string? invalidHash)
        {
            // Arrange
            var user = User.Create(Guid.NewGuid(), _validEmail, "09120000000", "Zeinab", "Dev", "old_hash");

            // Act
            var act = () => user.ChangePassword(invalidHash!);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("passwordHash cannot be null or empty.");
        }

        [Fact]
        public void ChangePassword_WhenUserIsRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var user = User.Create(Guid.NewGuid(), _validEmail, "09120000000", "Zeinab", "Dev", "old_hash");
            user.Remove();

            // Act
            var act = () => user.ChangePassword("new_hashed_password");

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Removed user cannot change password.");
        }

        [Fact]
        public void RecordLogin_WhenUserIsActive_ShouldUpdateLastLogin()
        {
            // Arrange
            var user = User.Create(Guid.NewGuid(), _validEmail, "09120000000", "Zeinab", "Dev", "hashed");

            // Act
            user.RecordLogin();

            // Assert
            user.LastLogin.Should().NotBeNull();
            user.LastLogin!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public void RecordLogin_WhenUserIsRemoved_ShouldThrowDomainException()
        {
            // Arrange
            var user = User.Create(Guid.NewGuid(), _validEmail, "09120000000", "Zeinab", "Dev", "hashed");
            user.Remove();

            // Act
            var act = () => user.RecordLogin();

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Removed user cannot login.");
        }
    }
}
