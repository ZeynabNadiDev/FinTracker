using FinTracker.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Domain.Enums;

namespace Transaction.Domain.Entities.Transaction.Events
{
    public sealed record TransactionCreated : DomainEvent<Guid>
    {
        public Guid WalletId { get; init; }
        public decimal Amount { get; init; }
        public TransactionType Type { get; init; }

        public TransactionCreated(Transaction transaction)
            : base(transaction.Id, transaction.Version)
        {
            WalletId = transaction.WalletId;
            Amount = transaction.Amount;
            Type = transaction.Type;
        }
    }

    public sealed record TransactionDeleted : DomainEvent<Guid>
    {
        public Guid WalletId { get; init; }
        public decimal Amount { get; init; }
        public TransactionType Type { get; init; }

        public TransactionDeleted(Transaction transaction)
            : base(transaction.Id, transaction.Version)
        {
            WalletId = transaction.WalletId;
            Amount = transaction.Amount;
            Type = transaction.Type;
        }
    }
}
