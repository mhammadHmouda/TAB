using System.Net;
using TAB.Application.Core.Exceptions;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Exceptions;
using TAB.Domain.Core.Primitives;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception Occured: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, errors) = GetHttpStatusCodeAndError(ex);

        context.Response.StatusCode = (int)statusCode;

        var response = new ApiResponse { Errors = errors, StatusCode = statusCode };

        await context.Response.WriteAsJsonAsync(response);
    }

    private static (
        HttpStatusCode statusCode,
        IReadOnlyCollection<Error> errors
    ) GetHttpStatusCodeAndError(Exception exception) =>
        exception switch
        {
            ValidationException validationException
                => (HttpStatusCode.BadRequest, validationException.Errors),
            DomainException domainException
                => (HttpStatusCode.BadRequest, new[] { domainException.Error }),
            _ => (HttpStatusCode.InternalServerError, new[] { DomainErrors.General.ServerError })
        };
}
