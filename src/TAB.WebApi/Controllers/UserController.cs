﻿using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.UserManagement.SearchUsers;
using TAB.Application.Features.UserManagement.UpdateProfile;
using TAB.Contracts.Features.UserManagement.Users;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Controllers;

/// <summary>
/// Controller for handling user related operations.
/// </summary>
[TokenValidation]
public class UserController : ApiController
{
    /// <summary>
    /// Updates a user.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateUserRequest">The update user request.</param>
    /// <response code="200">The user was updated successfully.</response>
    /// <response code="400">The user details are invalid.</response>
    /// <returns>The update user result.</returns>
    [HttpPut(ApiRoutes.Users.Update)]
    public async Task<IActionResult> Update(int id, UpdateUserRequest updateUserRequest) =>
        await Result
            .Create(updateUserRequest)
            .Ensure(request => request.Id == id, DomainErrors.General.UnProcessableRequest)
            .Map(request => new UpdateUserCommand(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Password
            ))
            .Bind(command => Mediator.Send(command))
            .Match(() => Ok("User updated successfully."), BadRequest);

    /// <summary>
    /// Searches for a user.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="filters">The user filters.</param>
    /// <param name="sorting">The user sorting.</param>
    /// <response code="200">The user was found successfully.</response>
    /// <response code="400">The user details are invalid.</response>
    /// <returns>The search user result.</returns>
    [HttpGet(ApiRoutes.Users.Search)]
    public async Task<IActionResult> Search(
        string? filters,
        string? sorting,
        int page = 1,
        int pageSize = 10
    ) =>
        await Result
            .Create(new SearchUsersQuery(page, pageSize, filters, sorting))
            .Bind(query => Mediator.Send(query))
            .Match(Ok, BadRequest);
}
