using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Application.DTOs;

namespace Transaction.Application.Queries.GetTransactionById
{
    public record GetTransactionByIdQuery(Guid TransactionId, Guid UserId) : IRequest<Result<TransactionDto>>;
}
