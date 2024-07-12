using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.Todos;
using TAB.WebApi.Contracts;
using TAB.WebApi.Infrastructure;

namespace TAB.WebApi.Controllers;

public class TodoController : ApiController
{
    // I need to implement the following endpoints:
    // - Post /todos
    // - Get /todos/{id}
    // - Post /todos/{id}/done

    // <summary>
    // This is a simple POST endpoint that creates a new todo.
    // </summary>
    // <param name="request">The request to create a new todo.</param>
    // <returns>A 201 Created response with the created todo.</returns>
    // <response code="201">Returns the created todo.</response>
    // <response code="400">Returns an error message if the request is invalid.</response>

    [HttpPost(ApiRoutes.Todos.Create)]
    public async Task<IActionResult> CreateTodo(CreateTodoRequest request)
    {
        var result = await Mediator.Send(
            new CreateTodoCommand(request.Title, request.Description, request.IsDone)
        );

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetTodo), new { id = result.Value.Id }, result.Value)
            : BadRequest(result.Error);
    }

    // <summary>
    // This is a simple GET endpoint that retrieves a todo by its id.
    // </summary>
    // <param name="id">The id of the todo to retrieve.</param>
    // <returns>A 200 OK response with the retrieved todo.</returns>
    // <response code="200">Returns the retrieved todo.</response>
    // <response code="404">Returns an error message if the todo is not found.</response>

    [HttpGet(ApiRoutes.Todos.GetById)]
    public async Task<IActionResult> GetTodo(int id)
    {
        var result = await Mediator.Send(new GetTodoQuery(id));

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
