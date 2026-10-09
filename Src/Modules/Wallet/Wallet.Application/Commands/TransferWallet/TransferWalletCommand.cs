using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Commands.TransferWallet
{
    public record TransferWalletCommand(
       Guid SourceWalletId,
       Guid DestinationWalletId,
       decimal Amount,
       Guid UserId) : IRequest;
}
