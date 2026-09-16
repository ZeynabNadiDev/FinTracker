using FinTracker.SharedKernel.Domain;
using FinTracker.SharedKernel.Exceptions;
using System;
using Wallet.Domain.Entities.Wallet.Events;

namespace Wallet.Domain.Entities.Wallet;

public class Wallet : AggregateRoot<Guid>
{
    public string Title { get; private set; } = null!;
    public decimal Balance { get; private set; }
    public Guid UserId { get; private set; }
    public bool IsRemoved { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected Wallet() : base(Guid.Empty) { }

    private Wallet(Guid id, string title, Guid userId) : base(id)
    {
        EnsureNotEmpty(title, nameof(title));
        Title = title.Trim();
        UserId = userId;
        Balance = 0;
        IsRemoved = false;
        CreatedAt = DateTime.UtcNow;
    }

    public static Wallet Create(Guid id, string title, Guid userId)
    {
        var wallet = new Wallet(id, title, userId);
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
