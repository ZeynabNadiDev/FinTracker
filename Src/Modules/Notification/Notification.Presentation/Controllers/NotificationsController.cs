using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Commands.MarkAllNotificationsAsRead;
using Notification.Application.Commands.MarkNotificationAsRead;
using Notification.Application.Queries.GetNotificationsByUserId;
using Notification.Application.Queries.GetUnreadNotificationCount;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly ISender _sender;

        public NotificationsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get notifications of current user",
            Description = "Retrieves paginated notifications for the currently authenticated user, optionally filtering unread ones.")]
        public async Task<IActionResult> GetAll(
            [FromQuery] bool onlyUnread = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId();
            var query = new GetNotificationsByUserIdQuery(userId, onlyUnread, pageNumber, pageSize);
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("unread-count")]
        [SwaggerOperation(
            Summary = "Get unread notifications count",
            Description = "Retrieves the total count of unread notifications for the currently authenticated user.")]
        public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId();
            var query = new GetUnreadNotificationCountQuery(userId);
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/read")]
        [SwaggerOperation(
            Summary = "Mark notification as read",
            Description = "Marks a specific notification as read for the currently authenticated user.")]
        public async Task<IActionResult> MarkAsRead(
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId();
            var command = new MarkNotificationAsReadCommand(id, userId);
            await _sender.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPatch("read-all")]
        [SwaggerOperation(
            Summary = "Mark all notifications as read",
            Description = "Marks all unread notifications as read for the currently authenticated user.")]
        public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId();
            var command = new MarkAllNotificationsAsReadCommand(userId);
            await _sender.Send(command, cancellationToken);

            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }
}
