using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Application.DTOs;

namespace Transaction.Application.Queries.GetTransactionsByWalletId
{
    public record GetTransactionsByWalletIdQuery(Guid WalletId, Guid UserId) : IRequest<Result<IReadOnlyList<TransactionDto>>>;
}
