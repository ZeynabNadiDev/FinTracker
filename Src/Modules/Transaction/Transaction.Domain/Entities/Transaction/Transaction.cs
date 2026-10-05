using FinTracker.SharedKernel.Domain;
using FinTracker.SharedKernel.Enums;
using FinTracker.SharedKernel.Events;
using FinTracker.SharedKernel.Exceptions;
using System;


namespace Transaction.Domain.Entities.Transaction
{
    public class Transaction : AggregateRoot<Guid>
    {
        public Guid UserId { get; private set; }
        public Guid WalletId { get; private set; }
        public Guid? DestinationWalletId { get; private set; }
        public int? CategoryId { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public string? Description { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public bool IsRemoved { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        protected Transaction() : base(Guid.Empty)
        {
        }

        private Transaction(
            Guid id,
            Guid userId,
            Guid walletId,
            Guid? destinationWalletId,
            int? categoryId,
            decimal amount,
            TransactionType type,
            string? description,
            DateTime transactionDate) : base(id)
        {
            if (userId == Guid.Empty)
                throw new DomainException("UserId cannot be empty.");

            if (walletId == Guid.Empty)
                throw new DomainException("WalletId cannot be empty.");

            if (amount <= 0)
                throw new DomainException("Amount must be greater than zero.");

            // Domain business rules validation
            if (type == TransactionType.Transfer)
            {
                if (!destinationWalletId.HasValue || destinationWalletId.Value == Guid.Empty)
                    throw new DomainException("Destination wallet is required for transfer transactions.");

                if (destinationWalletId.Value == walletId)
                    throw new DomainException("Destination wallet cannot be the same as source wallet.");
            }
            else
            {
                if (!categoryId.HasValue || categoryId.Value <= 0)
                    throw new DomainException("CategoryId must be a valid positive identifier for non-transfer transactions.");
            }

            UserId = userId;
            WalletId = walletId;
            DestinationWalletId = destinationWalletId;
            CategoryId = categoryId;
            Amount = amount;
            Type = type;
            Description = description?.Trim();
            TransactionDate = transactionDate == default ? DateTime.UtcNow : transactionDate;
            IsRemoved = false;
            CreatedAt = DateTime.UtcNow;
        }

        // Factory method for income / expense transactions
        public static Transaction Create(
            Guid userId,
            Guid walletId,
            int categoryId,
            decimal amount,
            TransactionType type,
            string? description,
            DateTime transactionDate)
        {
            if (type == TransactionType.Transfer)
                throw new DomainException("Use CreateTransfer method for transfer transactions.");

            var transaction = new Transaction(
                Guid.NewGuid(),
                userId,
                walletId,
                null,
                categoryId,
                amount,
                type,
                description,
                transactionDate);

            // Pass primitive properties instead of passing the entire entity instance
            transaction.AddDomainEvent(new TransactionCreatedEvent(
                transaction.Id,
                transaction.WalletId,
                null,
                transaction.Amount,
                transaction.Type,
                transaction.CategoryId
            ));

            return transaction;
        }

        // Factory method dedicated for transfer transactions
        public static Transaction CreateTransfer(
            Guid userId,
            Guid sourceWalletId,
            Guid destinationWalletId,
            decimal amount,
            string? description = null,
            DateTime transactionDate = default)
        {
            var transaction = new Transaction(
                Guid.NewGuid(),
                userId,
                sourceWalletId,
                destinationWalletId,
                null,
                amount,
                TransactionType.Transfer,
                description ?? $"Transfer to wallet {destinationWalletId}",
                transactionDate);

            // Pass primitive properties for transfer event
            transaction.AddDomainEvent(new TransactionCreatedEvent(
                transaction.Id,
                transaction.WalletId,
                transaction.DestinationWalletId,
                transaction.Amount,
                transaction.Type,
                null
            ));

            return transaction;
        }

        public long Remove()
        {
            if (IsRemoved)
                throw new DomainException("Transaction is already removed.");

            IsRemoved = true;
            UpdatedAt = DateTime.UtcNow;

            // Pass primitive properties for deleted event
            AddDomainEvent(new TransactionDeletedEvent(
                Id,
                WalletId,
                Amount,
                Type
            ));

            return Version;
        }
    }
}
