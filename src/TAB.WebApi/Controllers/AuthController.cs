using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.UserManagement.Register;
using TAB.Contracts.Features.UserManagement;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Controllers;

/// <summary>
/// Controller for handling authentication related operations.
/// </summary>
public class AuthController : ApiController
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="createUserRequest">The create user request.</param>
    /// <response code="200">The user was created successfully.</response>
    /// <response code="400">The user already exists.</response>
    /// <response code="400"> The user details are invalid.</response>
    /// <returns>The creation user result.</returns>
    [HttpPost(ApiRoutes.Users.Register)]
    public async Task<IActionResult> Create(CreateUserRequest createUserRequest) =>
        await Result
            .Create(createUserRequest)
            .Map(request => new CreateUserCommand(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.Role
            ))
            .Bind(command => Mediator.Send(command))
            .Match(
                user =>
                    Ok(
                        user,
                        "Account Created Successfully. Please check your email for activation."
                    ),
                BadRequest
            );
}
