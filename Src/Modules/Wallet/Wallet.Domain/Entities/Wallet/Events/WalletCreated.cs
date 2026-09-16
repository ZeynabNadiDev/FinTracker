using FinTracker.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Domain.Entities.Wallet.Events
{
    public sealed record WalletCreated : DomainEvent<Guid>
    {
        public WalletCreated(Wallet wallet) : base(wallet.Id, wallet.Version) { }
    }
    public sealed record WalletUpdated : DomainEvent<Guid>
    {
        public WalletUpdated(Wallet wallet) : base(wallet.Id, wallet.Version) { }
    }
    public sealed record WalletDeleted : DomainEvent<Guid>
    {
        public WalletDeleted(Wallet wallet) : base(wallet.Id, wallet.Version) { }
    }

}
