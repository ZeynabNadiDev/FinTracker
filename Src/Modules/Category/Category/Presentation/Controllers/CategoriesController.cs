using Category.Application.Commands.CreateCategory;
using Category.Application.Commands.CreateCategory.Handler;
using Category.Application.Commands.DeleteCategory;
using Category.Application.Commands.UpdateCategory;
using Category.Application.Commands.UpdateCategory.Handler;
using Category.Application.Queries.GetCategoriesByUser;
using Category.Application.Queries.GetCategoriesByUser.Handler;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Category.Presentation.Controllers;

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
      Description = "Creates a new income or expense category for the specified user.")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpGet]
    [SwaggerOperation(
         Summary = "Get categories by user",
         Description = "Retrieves all categories associated with a specific user ID.")]
    public async Task<IActionResult> GetByUser([FromQuery] Guid userId)
    {
        var query = new GetCategoriesByUserQuery(userId);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(
         Summary = "Update category",
         Description = "Updates the details of an existing category by ID.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
    {
        if (id != command.CategoryId)
        {
            return BadRequest("Category ID mismatch between URL and body.");
        }

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(
       Summary = "Delete category",
       Description = "Deletes a category from the system.")]
    public async Task<IActionResult> Delete(int id, [FromQuery] Guid userId)
    {
        var command = new DeleteCategoryCommand(id, userId);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }
}
