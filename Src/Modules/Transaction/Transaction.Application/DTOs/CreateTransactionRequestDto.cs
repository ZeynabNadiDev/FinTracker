using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Application.DTOs
{
    public sealed record CreateTransactionRequestDto(
         Guid WalletId,
         int CategoryId,
         decimal Amount,
         int Type,
         string? Description);
}
