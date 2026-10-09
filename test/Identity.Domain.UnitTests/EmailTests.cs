using FinTracker.SharedKernel.Exceptions;
using FluentAssertions;
using Identity.Domain.ValueObject;
using Xunit;

namespace Identity.Domain.UnitTests
{
    public class EmailTests
    {
        [Theory]
        [InlineData("test@example.com", "test@example.com")]
        [InlineData("  user@domain.com  ", "user@domain.com")]
        [InlineData("User.Name+Tag@Domain.COM", "user.name+tag@domain.com")]
        public void Constructor_WithValidEmail_ShouldNormalizeToLowerAndTrim(string input, string expected)
        {
            // Act
            var email = new Email(input);

            // Assert
            email.Value.Should().Be(expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithNullOrWhiteSpace_ShouldThrowDomainException(string? invalidEmail)
        {
            // Act
            var act = () => new Email(invalidEmail!);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Email cannot be null or empty.");
        }

        [Theory]
        [InlineData("plainaddress")]
        [InlineData("missingatsign.com")]
        [InlineData("@missingusername.com")]
        [InlineData("username@.com")]
        [InlineData("user name@example.com")]
        public void Constructor_WithInvalidEmailFormat_ShouldThrowDomainException(string invalidEmail)
        {
            // Act
            var act = () => new Email(invalidEmail);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Invalid email format.");
        }
    }
}
