using FinTracker.SharedKernel.Domain;
using FinTracker.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Transaction.Domain.Entities.Transaction.Events
{
    public sealed record TransactionCreatedEvent(
      Guid TransactionId,
      Guid WalletId,
      Guid? DestinationWalletId,
      decimal Amount,
     TransactionType Type,
     int? CategoryId,
     long Version = 0
 ) : DomainEvent<Guid>(TransactionId, Version);

    public sealed record TransactionDeletedEvent(
        Guid TransactionId,
        Guid WalletId,
        decimal Amount,
        TransactionType Type,
        long Version = 0
    ) : DomainEvent<Guid>(TransactionId, Version);
}
