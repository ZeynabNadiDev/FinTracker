using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.DTO;

namespace Wallet.Application.Commands.UpdateWallet
{
    public sealed record UpdateWalletCommand(
     Guid WalletId,
     Guid UserId,
     string Title
 ) : IRequest<Result<WalletResponseDto>>;
}
