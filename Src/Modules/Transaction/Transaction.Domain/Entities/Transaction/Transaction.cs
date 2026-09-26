using FinTracker.SharedKernel.Domain;
using FinTracker.SharedKernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Domain.Entities.Transaction.Events;
using Transaction.Domain.Enums;

namespace Transaction.Domain.Entities.Transaction
{
    public class Transaction: AggregateRoot<Guid>
    {
        public Guid UserId { get;private set; }
        public Guid WalletId { get;private set; }
        public int CategoryId { get; private set; }
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
            int categoryId,
            decimal amount,
            TransactionType type,
            string? description,
            DateTime transactionDate) : base(id)
        {
            if (userId == Guid.Empty)
                throw new DomainException("UserId cannot be empty.");

            if (walletId == Guid.Empty)
                throw new DomainException("WalletId cannot be empty.");

            if (categoryId <= 0)
                throw new DomainException("CategoryId must be a valid positive identifier.");

            if (amount <= 0)
                throw new DomainException("Amount must be greater than zero.");

            UserId = userId;
            WalletId = walletId;
            CategoryId = categoryId;
            Amount = amount;
            Type = type;
            Description = description?.Trim();
            TransactionDate = transactionDate == default ? DateTime.UtcNow : transactionDate;
            IsRemoved = false;
            CreatedAt = DateTime.UtcNow;
        }

        public static Transaction Create(
            Guid userId,
            Guid walletId,
            int categoryId,
            decimal amount,
            TransactionType type,
            string? description,
            DateTime transactionDate)
        {
            var transaction = new Transaction(
                Guid.NewGuid(),
                userId,
                walletId,
                categoryId,
                amount,
                type,
                description,
                transactionDate);

            transaction.AddDomainEvent(new TransactionCreated(transaction));

            return transaction;
        }

        public long Remove()
        {
            if (IsRemoved)
                throw new DomainException("Transaction is already removed.");

            IsRemoved = true;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new TransactionDeleted(this));

            return Version;
        }
    }
}
