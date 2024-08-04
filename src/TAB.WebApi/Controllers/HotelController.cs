using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.HotelManagement.Amenities.AddAmenity;
using TAB.Application.Features.HotelManagement.Hotels.AddHotels;
using TAB.Application.Features.HotelManagement.Hotels.GetFeaturedDeals;
using TAB.Application.Features.HotelManagement.Hotels.GetHotelById;
using TAB.Application.Features.HotelManagement.Hotels.GetHotelGallery;
using TAB.Application.Features.HotelManagement.Hotels.GetRecentVisits;
using TAB.Application.Features.HotelManagement.Hotels.SearchHotels;
using TAB.Application.Features.HotelManagement.Hotels.UpdateHotels;
using TAB.Application.Features.HotelManagement.Images.UploadImages;
using TAB.Application.Features.HotelManagement.Rooms.AddRoom;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Core.Enums;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
using TAB.WebApi.Contracts;
using TAB.WebApi.Extensions;

namespace TAB.WebApi.Controllers;

/// <summary>
/// Controller for managing hotels.
/// </summary>
[TokenValidation]
[Authorize(Roles = "Admin")]
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

    /// <summary>
    /// Updates a hotel.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <param name="request">The request to update the hotel.</param>
    /// <returns>The updated hotel.</returns>
    /// <response code="200">The hotel was updated successfully.</response>
    /// <response code="400">The request is invalid.</response>
    [HttpPut(ApiRoutes.Hotels.Update)]
    public async Task<IActionResult> Update(int id, UpdateHotelRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.Id, DomainErrors.General.UnProcessableRequest)
            .Map(x => new UpdateHotelCommand(
                x.id,
                x.request.Name,
                x.request.Description,
                x.request.Latitude,
                x.request.Longitude
            ))
            .Bind(x => Mediator.Send(x))
            .Match(() => Ok("Hotel updated successfully."), BadRequest);

    /// <summary>
    /// Creates amenities for a hotel.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <param name="request">The create amenity request.</param>
    /// <response code="200">The amenities were created successfully.</response>
    /// <response code="400">The amenities were not created successfully.</response>
    /// <returns>The result of the create amenity operation.</returns>
    [HttpPost(ApiRoutes.Hotels.AddAmenity)]
    public async Task<IActionResult> CreateAmenities(int id, CreateAmenityRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.TypeId, DomainErrors.General.UnProcessableRequest)
            .Map(x => new CreateAmenityCommand(
                x.request.Name,
                x.request.Description,
                AmenityType.Hotel,
                x.request.TypeId
            ))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Creates a room for a hotel.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <param name="request">The create room request.</param>
    /// <response code="200">The room was created successfully.</response>
    /// <response code="400">The room was not created successfully.</response>
    /// <returns>The result of the create room operation.</returns>
    [HttpPost(ApiRoutes.Hotels.CreateRoom)]
    public async Task<IActionResult> CreateRoom(int id, CreateRoomRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.HotelId, DomainErrors.General.UnProcessableRequest)
            .Map(x => new CreateRoomCommand(
                x.request.HotelId,
                x.request.Number,
                x.request.Description,
                x.request.Price,
                x.request.Currency,
                x.request.Type,
                x.request.CapacityOfAdults,
                x.request.CapacityOfChildren
            ))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Search hotels with dynamic filters and sorting.
    /// </summary>
    /// <param name="filters">The filters to apply.</param>
    /// <param name="sorting">The sorting to apply.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The hotels with reviews.</returns>
    /// <response code="200">The hotels with reviews.</response>
    /// <response code="400">The request is invalid.</response>
    [HttpGet(ApiRoutes.Hotels.Search)]
    public async Task<IActionResult> SearchHotels(
        string? filters,
        string? sorting,
        int page = 1,
        int pageSize = 10
    ) =>
        await Result
            .Create((filters, sorting, page, pageSize))
            .Map(x => new SearchHotelsQuery(x.filters, x.sorting, x.page, x.pageSize))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Get a hotel by ID.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <returns>The hotel with all its details.</returns>
    /// <response code="200">The hotel with all its details.</response>
    /// <response code="400">The request is invalid.</response>
    [HttpGet(ApiRoutes.Hotels.Get)]
    public async Task<IActionResult> GetHotelById(int id) =>
        await Result
            .Create(id)
            .Map(x => new GetHotelByIdQuery(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Get featured deals for hotels.
    /// </summary>
    /// <param name="limit">The limit of hotels to get.</param>
    /// <returns>The featured deals for hotels.</returns>
    /// <response code="200">The featured deals for hotels.</response>
    /// <response code="400">The request is invalid.</response>
    [HttpGet(ApiRoutes.Hotels.FeaturedDeals)]
    public async Task<IActionResult> GetFeaturedDeals(int limit) =>
        await Result
            .Create(limit)
            .Map(x => new GetFeaturedDealsQuery(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Get recent visits for a user.
    /// </summary>
    /// <param name="limit">The limit of recent visits to get.</param>
    /// <returns>The recent visits for a user.</returns>
    /// <response code="200">The recent visits for a user.</response>
    /// <response code="400">The request is invalid.</response>
    [HttpGet(ApiRoutes.Hotels.RecentVisits)]
    public async Task<IActionResult> GetRecentVisits(int limit) =>
        await Result
            .Create(limit)
            .Map(x => new GetRecentVisitsQuery(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Gets the gallery of a hotel.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <param name="page">The page number of the gallery.</param>
    /// <param name="pageSize">The page size of the gallery.</param>
    /// <response code="200">The gallery of the hotel was retrieved successfully.</response>
    /// <response code="400">The gallery of the hotel was not retrieved successfully.</response>
    /// <returns>The gallery of the hotel.</returns>
    [HttpGet(ApiRoutes.Hotels.Gallery)]
    [AllowAnonymous]
    public async Task<IActionResult> GetGallery(int id, int page = 1, int pageSize = 10) =>
        await Result
            .Create(new GetHotelGalleryQuery(id, page, pageSize))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
