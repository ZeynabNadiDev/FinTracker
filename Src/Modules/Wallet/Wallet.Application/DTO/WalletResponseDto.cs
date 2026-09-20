using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.DTO
{
    public sealed record CreateWalletRequest(string Title, string Currency = "IRT");
    public sealed record UpdateWalletRequest(string Title);

    public sealed record WalletResponseDto(
        Guid Id,
        string Title,
        decimal Balance,
        string Currency,
        Guid UserId
    );

}
