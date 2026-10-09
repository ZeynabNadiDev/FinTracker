using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.DTO;

namespace Wallet.Application.Queries.GetWalletsByUserId
{
    public record GetWalletsByUserIdQuery(Guid UserId) : IRequest<Result<IReadOnlyList<WalletResponseDto>>>;
}
