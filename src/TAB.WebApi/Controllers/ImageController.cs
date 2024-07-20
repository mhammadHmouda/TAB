using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.HotelManagement.Images.UpdateImage;
using TAB.Application.Features.HotelManagement.Images.UploadImages;
using TAB.Domain.Core.Enums;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Contracts;
using TAB.WebApi.Extensions;

namespace TAB.WebApi.Controllers;

/// <summary>
/// Controller for managing images.
/// </summary>
public class ImageController : ApiController
{
    /// <summary>
    /// Uploads images.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="type">The type of the image.</param>
    /// <param name="files">The collection of files to upload.</param>
    /// <response code="200">The images were uploaded successfully.</response>
    /// <response code="400">The images were not uploaded successfully.</response>
    /// <returns>The result of the upload operation.</returns>
    [HttpPost(ApiRoutes.Images.Upload)]
    public async Task<IActionResult> UploadImages(
        int id,
        ImageType type,
        [FromForm] IFormCollection files
    ) =>
        await Result
            .Create((id, type, files))
            .Map(x => new UploadImagesCommand(x.id, x.type, x.files.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Updates an image.
    /// </summary>
    /// <param name="id">The ID of the image.</param>
    /// <param name="file">The file to upload.</param>
    /// <response code="200">The image was updated successfully.</response>
    /// <response code="400">The image was not updated successfully.</response>
    /// <returns>The result of the update operation.</returns>
    [HttpPut(ApiRoutes.Images.Update)]
    public async Task<IActionResult> UpdateImage(int id, [FromForm] IFormFile file) =>
        await Result
            .Create((id, file))
            .Map(x => new UpdateImageCommand(x.id, x.file.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
