using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.HotelManagement.AddHotels;
using TAB.Contracts.Features.HotelManagement;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Controllers;

/// <summary>
/// Controller for managing hotels.
/// </summary>
[TokenValidation]
public class HotelController : ApiController
{
    /// <summary>
    /// Creates a new hotel.
    /// </summary>
    /// <param name="createHotelRequest">The request to create a hotel.</param>
    /// <returns>The created hotel.</returns>
    /// <response code="200">The hotel was created successfully.</response>
    /// <response code="400">The request is invalid.</response>

    [HttpPost(ApiRoutes.Hotels.Create)]
    public async Task<IActionResult> CreateHotel(
        [FromBody] CreateHotelRequest createHotelRequest
    ) =>
        await Result
            .Create(createHotelRequest)
            .Map(request => new CreateHotelCommand(
                request.Name,
                request.Description,
                request.Latitude,
                request.Longitude,
                request.CityId,
                request.OwnerId,
                request.Type
            ))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);
}
