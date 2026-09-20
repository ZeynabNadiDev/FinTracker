using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Commands.DeleteWallet
{
    public sealed record DeleteWalletCommand(
     Guid WalletId,
     Guid UserId
 ) : IRequest<Result>;
}
