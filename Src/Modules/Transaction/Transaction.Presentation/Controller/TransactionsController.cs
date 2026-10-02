using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Transaction.Application.Commands.CreateTransaction;
using Transaction.Application.Commands.DeleteTransaction;
using Transaction.Application.DTOs;
using Transaction.Application.Queries.GetTransactionById;
using Transaction.Application.Queries.GetTransactions;
using Transaction.Application.Queries.GetTransactionsByUserId;
using Transaction.Application.Queries.GetTransactionsByWalletId;
using Transaction.Domain.Enums;

namespace Transaction.Presentation.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ISender _sender;

        public TransactionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new transaction",
            Description = "Creates a new transaction for the currently authenticated user.")]
        public async Task<IActionResult> Create(
            [FromBody] CreateTransactionRequest request,
            CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();

            var command = new CreateTransactionCommand(
                userId,
                request.WalletId,
                request.CategoryId,
                request.Amount,
                (TransactionType)request.Type,
                request.Description);

            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpDelete("{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete transaction",
            Description = "Deletes a transaction belonging to the currently authenticated user.")]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();

            var command = new DeleteTransactionCommand(id, userId);
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(
            Summary = "Get transaction by ID",
            Description = "Retrieves a single transaction by its unique ID for the currently authenticated user.")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();

            var query = new GetTransactionByIdQuery(id, userId);
            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get transactions with pagination and filters",
            Description = "Retrieves paged transactions for the currently authenticated user with optional filtering by wallet, category, and date range.")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? walletId = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId();

            var query = new GetTransactionsQuery(
                userId,
                pageNumber,
                pageSize,
                walletId,
                categoryId,
                fromDate,
                toDate);

            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpGet("wallet/{walletId:guid}")]
        [SwaggerOperation(
            Summary = "Get transactions by wallet ID",
            Description = "Retrieves all transactions belonging to a specific wallet for the currently authenticated user.")]
        public async Task<IActionResult> GetByWalletId(
            Guid walletId,
            CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();

            var query = new GetTransactionsByWalletIdQuery(walletId, userId);
            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpGet("all")]
        [SwaggerOperation(
            Summary = "Get all transactions of current user",
            Description = "Retrieves all transactions associated with the currently authenticated user without pagination.")]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();

            var query = new GetTransactionsByUserIdQuery(userId);
            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userIdClaim, out var userId)
                ? userId
                : Guid.Empty;
        }
    }
}
