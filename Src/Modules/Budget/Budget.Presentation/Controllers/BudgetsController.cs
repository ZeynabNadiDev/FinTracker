using Budget.Application.Commands.CreateBudget;
using Budget.Application.Commands.UpdateBudgetAmount;
using Budget.Application.DTOs;
using Budget.Application.Queries.GetBudgets;
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

namespace Budget.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetsController : ControllerBase
    {
        private readonly ISender _sender;

        public BudgetsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get budgets of current user by period",
            Description = "Retrieves all budgets associated with the currently authenticated user for a specific month and year.")]
        public async Task<IActionResult> GetBudgets([FromQuery] int month, [FromQuery] int year, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var query = new GetBudgetsQuery(userId, month, year);
            var result = await _sender.Send(query, cancellationToken);

            return !result.IsSuccess ? BadRequest(result.Error) : Ok(result.Value);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new budget",
            Description = "Creates a new budget for a specific category and period for the currently authenticated user.")]
        public async Task<IActionResult> Create([FromBody] CreateBudgetRequest request, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var command = new CreateBudgetCommand(
                userId,
                request.CategoryId,
                request.Amount,
                request.Month,
                request.Year);

            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("{id:guid}/amount")]
        [SwaggerOperation(
            Summary = "Update budget amount",
            Description = "Updates the target amount of an existing budget belonging to the currently authenticated user.")]
        public async Task<IActionResult> UpdateAmount(Guid id, [FromBody] UpdateBudgetAmountRequest request, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var command = new UpdateBudgetAmountCommand(id, userId, request.Amount);
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }
}
