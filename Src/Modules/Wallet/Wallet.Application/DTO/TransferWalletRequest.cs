using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.DTO
{
    public sealed record TransferWalletRequest(
      Guid SourceWalletId,
      Guid DestinationWalletId,
      decimal Amount
  );
}
