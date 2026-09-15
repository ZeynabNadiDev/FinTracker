using FinTracker.SharedKernel.Domain;
using FinTracker.SharedKernel.Exceptions;
using Identity.Domain.Entities.User.Events;
using Identity.Domain.ValueObject;

namespace Identity.Domain.Entities.User;

public class User : AggregateRoot<Guid>
{
    public Email Email { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public bool IsRemoved { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? LastLogin { get; private set; }

    protected User() : base(Guid.Empty)
    {
    }

    private User(
        Guid id,
        Email email,
        string phoneNumber,
        string firstName,
        string lastName,
        string passwordHash) : base(id)
    {
        Email = email;
        PhoneNumber = phoneNumber.Trim();
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        PasswordHash = passwordHash;
        IsRemoved = false;
        CreatedAt = DateTime.UtcNow;
    }

    public static User Create(
        Guid id,
        Email email,
        string phoneNumber,
        string firstName,
        string lastName,
        string passwordHash)
    {
        EnsureNotEmpty(phoneNumber, nameof(phoneNumber));
        EnsureNotEmpty(firstName, nameof(firstName));
        EnsureNotEmpty(lastName, nameof(lastName));
        EnsureNotEmpty(passwordHash, nameof(passwordHash));

        var user = new User(id, email, phoneNumber, firstName, lastName, passwordHash);

        user.AddDomainEvent(new UserCreated(user));

        return user;
    }

    public long Update(
        Email email,
        string phoneNumber,
        string firstName,
        string lastName)
    {
        if (IsRemoved)
            throw new DomainException("Removed user cannot be updated.");

        EnsureNotEmpty(phoneNumber, nameof(phoneNumber));
        EnsureNotEmpty(firstName, nameof(firstName));
        EnsureNotEmpty(lastName, nameof(lastName));

        Email = email;
        PhoneNumber = phoneNumber.Trim();
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserUpdated(this));

        return Version;
    }

    public long Remove()
    {
        if (IsRemoved)
            throw new DomainException("User is already removed.");

        IsRemoved = true;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserDeleted(this));

        return Version;
    }

    public long ChangePassword(string passwordHash)
    {
        if (IsRemoved)
            throw new DomainException("Removed user cannot change password.");

        EnsureNotEmpty(passwordHash, nameof(passwordHash));

        PasswordHash = passwordHash;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserPasswordChanged(this));

        return Version;
    }

    public long RecordLogin()
    {
        if (IsRemoved)
            throw new DomainException("Removed user cannot login.");

        LastLogin = DateTime.UtcNow;

        return Version;
    }

    private static void EnsureNotEmpty(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(
                $"{fieldName} cannot be null or empty.");
    }
}
