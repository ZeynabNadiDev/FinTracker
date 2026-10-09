using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.DTO;

namespace Wallet.Application.Commands.CreateWallet
{
    public record CreateWalletCommand(
      Guid UserId,
      string Title,
      string Currency
  ) : IRequest<Result<WalletResponseDto>>;
}
