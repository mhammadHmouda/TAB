using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.HotelManagement.Cities.AddCity;
using TAB.Application.Features.HotelManagement.Images.UploadImages;
using TAB.Contracts.Features.HotelManagement.Cities;
using TAB.Domain.Core.Enums;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
using TAB.WebApi.Contracts;
using TAB.WebApi.Extensions;

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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCity([FromBody] CreateCityRequest request)
    {
        return await Result
            .Create(request)
            .Map(city => new CreateCityCommand(city.Name, city.Country, city.PostOffice))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);
    }

    /// <summary>
    /// Upload images for a city.
    /// </summary>
    /// <param name="id">The ID of the city.</param>
    /// <param name="files">The collection of files to upload.</param>
    /// <response code="200">The image was uploaded successfully.</response>
    /// <response code="400">The image was not uploaded successfully.</response>
    /// <returns>The result of the upload operation.</returns>
    [HttpPost(ApiRoutes.Cities.UploadImages)]
    public async Task<IActionResult> UploadImages(int id, [FromForm] IFormCollection files)
    {
        return await Result
            .Create((id, files))
            .Map(x => new UploadImagesCommand(x.id, ImageType.City, x.files.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
    }
}
