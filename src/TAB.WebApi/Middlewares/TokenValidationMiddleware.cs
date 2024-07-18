using TAB.Application.Core.Interfaces.Common;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Middlewares;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TokenValidationMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;

        if (ShouldSkipTokenValidation(path!))
        {
            await _next(context);
            return;
        }

        if (context.Request.Headers.TryGetValue("Authorization", out var tokenValue))
        {
            var token = tokenValue.ToString().Split(" ").Last();

            using var scope = _serviceScopeFactory.CreateScope();

            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenValidationService>();
            var isValid = await tokenService.ValidateTokenAsync(token);

            if (!isValid)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }

    private static bool ShouldSkipTokenValidation(string path) =>
        path.Contains(ApiRoutes.Auth.Login)
        || path.Contains(ApiRoutes.Auth.Register)
        || path.Contains("swagger/index.html")
        || path.Contains("swagger/v1/swagger.json");
}
