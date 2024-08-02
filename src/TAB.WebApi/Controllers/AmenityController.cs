using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.HotelManagement.Amenities.DeleteAmenity;
using TAB.Application.Features.HotelManagement.Amenities.GetAmenities;
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
    /// <summary>
    /// Update an amenity.
    /// </summary>
    /// <param name="id">The ID of the amenity.</param>
    /// <param name="request">The request to update an amenity.</param>
    /// <returns>The updated amenity.</returns>
    /// <response code="200">The amenity was updated successfully.</response>
    /// <response code="400">The request is invalid.</response>
    /// <returns>The result of the update operation.</returns>
    [HttpPut(ApiRoutes.Amenities.Update)]
    public async Task<IActionResult> UpdateAmenity(int id, UpdateAmenityRequest request) =>
        await Result
            .Create(request)
            .Ensure(r => r.Id == id, DomainErrors.General.UnProcessableRequest)
            .Map(req => new UpdateAmenityCommand(req.Id, req.Name, req.Description))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Delete an amenity.
    /// </summary>
    /// <param name="id">The ID of the amenity.</param>
    /// <returns>The result of the delete operation.</returns>
    /// <response code="200">The amenity was deleted successfully.</response>
    /// <response code="400">The amenity was not found.</response>
    /// <returns>The result of the delete operation.</returns>
    [HttpDelete(ApiRoutes.Amenities.Delete)]
    public async Task<IActionResult> DeleteAmenity(int id) =>
        await Result
            .Create(id)
            .Map(i => new DeleteAmenityCommand(i))
            .Bind(command => Mediator.Send(command))
            .Match(() => Ok("The amenity was deleted successfully!"), BadRequest);

    /// <summary>
    /// Get all amenities.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="filters">The filters to apply.</param>
    /// <param name="sorting">The sorting to apply.</param>
    /// <returns>The amenities.</returns>
    /// <response code="200">The amenities were retrieved successfully.</response>
    /// <response code="400">The request is invalid.</response>
    [HttpGet(ApiRoutes.Amenities.Search)]
    public async Task<IActionResult> SearchAmenities(
        string? filters,
        string? sorting,
        int page = 1,
        int pageSize = 10
    ) =>
        await Result
            .Create((page, pageSize, filters, sorting))
            .Map(x => new GetAmenitiesQuery(x.page, pageSize, x.filters, x.sorting))
            .Bind(query => Mediator.Send(query))
            .Match(Ok, BadRequest);
}
