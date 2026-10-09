using FinTracker.SharedKernel.Domain;
using FinTracker.SharedKernel.Events;
using FinTracker.SharedKernel.Exceptions;
using FinTracker.SharedKernel.ValueObjects;
using System.ComponentModel.DataAnnotations;
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

    [Timestamp]
    public byte[] RowVersion { get; private set; } = null!;

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

        public void Deposit(decimal amount)
        {
            if (IsRemoved)
                throw new DomainException("Cannot deposit to a removed wallet.");

            if (amount <= 0)
                throw new DomainException("Deposit amount must be greater than zero.");

            Balance = new Money(Balance.Amount + amount, Balance.Currency);
            UpdatedAt = DateTime.UtcNow;
        }

        public void Withdraw(decimal amount)
        {
            if (IsRemoved)
                throw new DomainException("Cannot withdraw from a removed wallet.");

            if (amount <= 0)
                throw new DomainException("Withdraw amount must be greater than zero.");

            if (Balance.Amount < amount)
                throw new DomainException("Insufficient wallet balance.");

            Balance = new Money(Balance.Amount - amount, Balance.Currency);
            UpdatedAt = DateTime.UtcNow;
        }

        public void TransferTo(Wallet destinationWallet, decimal amount)
        {
            if (IsRemoved)
                throw new DomainException("Cannot transfer from a removed wallet.");

            if (destinationWallet is null)
                throw new DomainException("Destination wallet cannot be null.");

            if (destinationWallet.IsRemoved)
                throw new DomainException("Cannot transfer to a removed wallet.");

            if (Id == destinationWallet.Id)
                throw new DomainException("Cannot transfer money to the same wallet.");

            if (Balance.Currency != destinationWallet.Balance.Currency)
                throw new DomainException($"Currency mismatch: cannot transfer from {Balance.Currency} to {destinationWallet.Balance.Currency}.");

            // Perform transfer atomically in domain
            Withdraw(amount);
            destinationWallet.Deposit(amount);

            UpdatedAt = DateTime.UtcNow;

            // Raise domain event for transfer
            AddDomainEvent(new WalletTransferredEvent(SourceWalletId: Id,DestinationWalletId: destinationWallet.Id,
                UserId: UserId,Amount: amount));
        }



    }
}