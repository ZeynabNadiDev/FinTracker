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
using Wallet.Application.Commands.CreateWallet;
using Wallet.Application.Commands.DeleteWallet;
using Wallet.Application.Commands.UpdateWallet;
using Wallet.Application.DTO;
using Wallet.Application.Queries.GetWalletsByUserId;

namespace Wallet.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly ISender _sender;

        public WalletsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [SwaggerOperation(
        Summary = "Get wallets of current user",
        Description = "Retrieves all wallets associated with the currently authenticated user.")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var query = new GetWalletsByUserIdQuery(userId);
            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost]
        [SwaggerOperation(
        Summary = "Create a new wallet",
        Description = "Creates a new wallet for the currently authenticated user.")]
        public async Task<IActionResult> Create([FromBody] CreateWalletRequest request, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var command = new CreateWalletCommand(userId, request.Title, request.Currency);
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("{id:guid}")]
        [SwaggerOperation(
        Summary = "Update wallet",
        Description = "Updates an existing wallet belonging to the currently authenticated user.")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWalletRequest request, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var command = new UpdateWalletCommand(id, userId, request.Title);
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpDelete("{id:guid}")]
        [SwaggerOperation(
        Summary = "Delete wallet",
        Description = "Deletes a wallet belonging to the currently authenticated user.")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var command = new DeleteWalletCommand(id, userId);
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
