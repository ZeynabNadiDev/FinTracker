
using FinTracker.SharedKernel.Enums;

namespace Transaction.Application.DTOs
{
    public sealed record CreateTransactionRequest(
     Guid WalletId,
     int CategoryId,
     decimal Amount,
     TransactionType Type,
     string? Description);
}
