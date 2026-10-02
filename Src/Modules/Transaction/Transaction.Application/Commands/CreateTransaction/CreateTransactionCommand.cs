using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Application.DTOs;
using Transaction.Domain.Enums;

namespace Transaction.Application.Commands.CreateTransaction
{
    public sealed record CreateTransactionCommand(
        Guid UserId,
        Guid WalletId,
        int CategoryId,
        decimal Amount,
        TransactionType Type,
        string? Description) : IRequest<Result<TransactionDto>>;
}
