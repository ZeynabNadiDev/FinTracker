using Category.Application.Commands.CreateCategory;
using Category.Application.Commands.DeleteCategory;
using Category.Application.Commands.UpdateCategory;
using Category.Application.Queries.GetCategoriesByUser;
using Category.Domain.Entities.Category;
using Category.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Category.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new category",
        Description = "Creates a new category for the currently authenticated user.")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
            return Unauthorized();

        var command = new CreateCategoryCommand(
            request.Name,
            request.Description,
            userId.Value,
            request.Type
        );

        var result = await _mediator.Send(command);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get categories of current user",
        Description = "Retrieves all categories associated with the currently authenticated user.")]
    public async Task<IActionResult> GetByUser()
    {
        var userId = GetCurrentUserId();
        if (userId is null)
            return Unauthorized();

        var query = new GetCategoriesByUserQuery(userId.Value);
        var result = await _mediator.Send(query);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(
        Summary = "Update category",
        Description = "Updates an existing category belonging to the currently authenticated user.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
            return Unauthorized();

        var command = new UpdateCategoryCommand(
            id,
            userId.Value,
            request.Name,
            request.Description,
            request.Type
        );

        var result = await _mediator.Send(command);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(
        Summary = "Delete category",
        Description = "Deletes a category belonging to the currently authenticated user.")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
            return Unauthorized();

        var command = new DeleteCategoryCommand(id, userId.Value);
        var result = await _mediator.Send(command);

        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("sub")?.Value;

        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}

public sealed record CreateCategoryRequest(string Name, string? Description, CategoryType Type);
public sealed record UpdateCategoryRequest(string Name, string? Description, CategoryType Type);
