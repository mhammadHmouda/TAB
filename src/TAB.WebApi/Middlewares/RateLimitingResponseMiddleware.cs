using System.Net;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Middlewares;

public class RateLimitingResponseMiddleware : IMiddleware
{
    private readonly ILogger<RateLimitingResponseMiddleware> _logger;

    public RateLimitingResponseMiddleware(ILogger<RateLimitingResponseMiddleware> logger) =>
        _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);
        const HttpStatusCode tooManyRequests = HttpStatusCode.TooManyRequests;

        if (context.Response.StatusCode != (int)tooManyRequests)
            return;

        var requestMethod = context.Request.Method;
        var requestPath = context.Request.Path;
        var clientIp = context.Connection.RemoteIpAddress?.ToString();

        _logger.LogWarning(
            "Rate limit exceeded for endpoint {RequestPath} with method {RequestMethod} from IP {ClientIp}. Please try again later.",
            requestPath,
            requestMethod,
            clientIp
        );

        var response = new ApiResponse
        {
            Errors = new List<Error> { DomainErrors.General.RequestLimitExceeded },
            StatusCode = tooManyRequests,
            Message = "Request limit exceeded for this endpoint. Please try again later."
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}
