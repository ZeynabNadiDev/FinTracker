using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Report.Queries.GetFinancialPeriodSummary;
using Swashbuckle.AspNetCore.Annotations;

namespace Report.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ISender _sender;

        public ReportsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("summary")]
        [SwaggerOperation(
            Summary = "Get financial period summary",
            Description = "Retrieves an aggregated financial summary for the currently authenticated user within a specified period.")]
        public async Task<IActionResult> GetSummary(
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate,
            [FromQuery] Guid? walletId,
            CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();

            var query = new GetFinancialPeriodSummaryQuery(
                userId,
                startDate,
                endDate,
                walletId);

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
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
