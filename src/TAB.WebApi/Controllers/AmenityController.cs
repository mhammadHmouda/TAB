using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.HotelManagement.Amenities.UpdateAmenity;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Controllers;

/// <summary>
/// Controller for managing amenities.
/// </summary>
[TokenValidation]
[Authorize(Roles = "Admin")]
public class AmenityController : ApiController
{
    // I need to add endpoint for update amenity
    /// <summary>
    /// Update an amenity.
    /// </summary>
    /// <param name="id">The ID of the amenity.</param>
    /// <param name="request">The request to update an amenity.</param>
    /// <returns>The updated amenity.</returns>
    /// <response code="200">The amenity was updated successfully.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="404">The amenity was not found.</response>
    /// <returns>The result of the update operation.</returns>
    [HttpPut(ApiRoutes.Amenities.Update)]
    public async Task<IActionResult> UpdateAmenity(int id, UpdateAmenityRequest request) =>
        await Result
            .Create(request)
            .Ensure(r => r.Id == id, DomainErrors.General.UnProcessableRequest)
            .Map(req => new UpdateAmenityCommand(req.Id, req.Name, req.Description))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);
}
