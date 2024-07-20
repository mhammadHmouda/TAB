using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.HotelManagement.AddHotels;
using TAB.Application.Features.HotelManagement.UploadImages;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Domain.Core.Enums;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
using TAB.WebApi.Contracts;
using TAB.WebApi.Extensions;

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

    /// <summary>
    /// Upload images for a hotel.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <param name="files">The collection of files to upload.</param>
    /// <response code="200">The image was uploaded successfully.</response>
    /// <response code="400">The image was not uploaded successfully.</response>
    /// <returns>The result of the upload operation.</returns>
    [HttpPost(ApiRoutes.Hotels.UploadImages)]
    public async Task<IActionResult> UploadImages(int id, [FromForm] IFormCollection files) =>
        await Result
            .Create((id, files))
            .Map(x => new UploadImagesCommand(x.id, ImageType.Hotel, x.files.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
