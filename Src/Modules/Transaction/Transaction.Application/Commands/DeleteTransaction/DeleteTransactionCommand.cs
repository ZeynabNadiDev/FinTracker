using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Application.Commands.DeleteTransaction
{
    public sealed record DeleteTransactionCommand(Guid TransactionId, Guid UserId) : IRequest<Result>;
}
