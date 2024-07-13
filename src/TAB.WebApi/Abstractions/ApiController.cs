using System.Net;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TAB.Domain.Core.Shared;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Abstractions;

/// <summary>
/// Represents an API controller.
/// </summary>
[ApiController]
[Produces("application/json")]
[Route("api/v{version:ApiVersion}")]
[ApiVersion("1.0")]
public abstract class ApiController : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;

    /// <summary>
    /// Returns an OK response with the specified value and message.
    /// </summary>
    /// <param name="value">The value to include in the response.</param>
    /// <param name="message">The message to include in the response.</param>
    /// <returns>An OK response.</returns>
    protected IActionResult Ok(object value, string? message) =>
        base.Ok(
            new ApiResponse
            {
                Data = value,
                Message = message,
                StatusCode = HttpStatusCode.OK
            }
        );

    /// <summary>
    /// Returns an OK response with the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to include in the response.</param>
    /// <returns>An OK response.</returns>
    protected IActionResult Ok<TValue>(TValue? value) =>
        base.Ok(new ApiResponse { Data = value, StatusCode = HttpStatusCode.OK });

    /// <summary>
    /// Returns a bad request response with the specified error.
    /// </summary>
    /// <param name="error">The error to include in the response.</param>
    /// <returns>A bad request response.</returns>
    protected IActionResult BadRequest(Error error) =>
        BadRequest(
            new ApiResponse { Errors = new[] { error }, StatusCode = HttpStatusCode.BadRequest }
        );
}
