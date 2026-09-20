using FinTracker.SharedKernel.Domain;
using FinTracker.SharedKernel.Exceptions;
using FinTracker.SharedKernel.ValueObjects;
using Wallet.Domain.Entities.Wallet.Events;

namespace Wallet.Domain.Entities.Wallet 
{ 

public class Wallet : AggregateRoot<Guid>
{
    public string Title { get; private set; } = null!;
    public Money Balance { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public bool IsRemoved { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected Wallet() : base(Guid.Empty) { }

    private Wallet(Guid id, string title, Guid userId, string currency) : base(id)
    {
        EnsureNotEmpty(title, nameof(title));
        Title = title.Trim();
        UserId = userId;
        Balance = new Money(0, currency); // Initial balance is zero with selected currency
        IsRemoved = false;
        CreatedAt = DateTime.UtcNow;
    }

    public static Wallet Create(Guid id, string title, Guid userId, string currency = "IRT")
    {
        var wallet = new Wallet(id, title, userId, currency);
        wallet.AddDomainEvent(new WalletCreated(wallet));
        return wallet;
    }

    public long Update(string title)
    {
        if (IsRemoved) throw new DomainException("Removed wallet cannot be updated.");
        EnsureNotEmpty(title, nameof(title));

        Title = title.Trim();
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new WalletUpdated(this));
        return Version;
    }

    public long Remove()
    {
        if (IsRemoved) throw new DomainException("Wallet is already removed.");

        IsRemoved = true;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new WalletDeleted(this));
        return Version;
    }

    private static void EnsureNotEmpty(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{fieldName} cannot be null or empty.");
    }
  }
}