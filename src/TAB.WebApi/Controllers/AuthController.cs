﻿using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.UserManagement.Activation;
using TAB.Application.Features.UserManagement.Login;
using TAB.Application.Features.UserManagement.Logout;
using TAB.Application.Features.UserManagement.Register;
using TAB.Contracts.Features.UserManagement.Auth;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
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
    [HttpPost(ApiRoutes.Auth.Register)]
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

    /// <summary>
    /// Activates a user account.
    /// </summary>
    /// <param name="token">The activation token.</param>
    /// <response code="200">The user account was activated successfully.</response>
    /// <response code="400">The activation token is invalid.</response>
    /// <returns>The activation result.</returns>
    [HttpGet(ApiRoutes.Auth.Activate)]
    public async Task<IActionResult> Activate([FromQuery] string token) =>
        await Result
            .Create(token)
            .Map(t => new ActivateUserCommand(t))
            .Bind(command => Mediator.Send(command))
            .Match(() => Ok("Account Activated Successfully. You can now login."), BadRequest);

    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="loginRequest">The login request.</param>
    /// <response code="200">The user was logged in successfully.</response>
    /// <response code="400">The user email or password are invalid.</response>
    /// <returns>The login result.</returns>
    [HttpPost(ApiRoutes.Auth.Login)]
    public async Task<IActionResult> Login(LoginRequest loginRequest) =>
        await Result
            .Create(loginRequest)
            .Map(request => new LoginUserCommand(request.Email, request.Password))
            .Bind(command => Mediator.Send(command))
            .Match(user => Ok(user, "Logged in successfully."), BadRequest);

    /// <summary>
    /// Logs out a user.
    /// </summary>
    /// <response code="200">The user was logged out successfully.</response>
    /// <response code="400">The logout token is invalid.</response>
    /// <returns>The logout result.</returns>
    [HttpPost(ApiRoutes.Auth.Logout)]
    [TokenValidation]
    public async Task<IActionResult> Logout()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ").Last();

        return await Result
            .Create(token)
            .Map(t => new LogoutUserCommand(t))
            .Bind(command => Mediator.Send(command))
            .Match(() => Ok("Logged out successfully."), BadRequest);
    }
}
