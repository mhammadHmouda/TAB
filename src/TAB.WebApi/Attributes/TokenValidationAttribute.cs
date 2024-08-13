using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TAB.Application.Core.Interfaces.Common;

namespace TAB.WebApi.Attributes;

public class TokenValidationAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var endpoint = context.ActionDescriptor.EndpointMetadata;
        var allowAnonymous = endpoint.OfType<AllowAnonymousAttribute>().Any();

        if (allowAnonymous)
        {
            return;
        }

        var tokenValue = context.HttpContext.Request.Headers["Authorization"];

        var token = tokenValue.ToString().Split(" ").Last();

        var tokenService =
            context.HttpContext.RequestServices.GetRequiredService<ITokenValidationService>();

        var isValid = await tokenService.ValidateTokenAsync(token);

        if (!isValid)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Result = new UnauthorizedResult();
        }
    }
}
