using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.HotelManagement.AddCity;
using TAB.Contracts.Features.HotelManagement;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Controllers;

/// <summary>
/// City controller.
/// </summary>
[TokenValidation]
public class CityController : ApiController
{
    /// <summary>
    /// Create a new city.
    /// </summary>
    /// <param name="request">The request to create a city.</param>
    /// <returns>The created city.</returns>
    /// <response code="200">The city was created successfully.</response>
    /// <response code="400">The request is invalid.</response>
    [HttpPost(ApiRoutes.Cities.Create)]
    public async Task<IActionResult> CreateCity([FromBody] CreateCityRequest request)
    {
        return await Result
            .Create(request)
            .Map(city => new CreateCityCommand(city.Name, city.Country, city.PostOffice))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);
    }
}
