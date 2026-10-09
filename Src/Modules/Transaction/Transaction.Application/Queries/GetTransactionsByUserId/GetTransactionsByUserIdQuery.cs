using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Application.DTOs;

namespace Transaction.Application.Queries.GetTransactionsByUserId
{
    public record GetTransactionsByUserIdQuery(Guid UserId) : IRequest<Result<IReadOnlyList<TransactionDto>>>;
}
