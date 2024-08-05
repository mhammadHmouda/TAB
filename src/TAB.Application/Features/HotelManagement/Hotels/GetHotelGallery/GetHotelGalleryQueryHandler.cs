using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Hotels.GetHotelGallery;

public class GetHotelGalleryQueryHandler
    : IQueryHandler<GetHotelGalleryQuery, Result<ImageResponse[]>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageRepository _imageRepository;

    public GetHotelGalleryQueryHandler(
        IHotelRepository hotelRepository,
        IImageRepository imageRepository
    )
    {
        _hotelRepository = hotelRepository;
        _imageRepository = imageRepository;
    }

    public async Task<Result<ImageResponse[]>> Handle(
        GetHotelGalleryQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new HotelWithRoomsSpecification(request.Id);

        var hotelMaybe = await _hotelRepository.GetAsync(spec, cancellationToken);

        if (hotelMaybe.HasNoValue)
        {
            return DomainErrors.Hotel.NotFound;
        }

        var hotel = hotelMaybe.Value;

        var roomImagesIds = hotel.Rooms.Select(r => r.Id).ToList();

        var hotelImages = (
            await _imageRepository.GetByHotelIdAsync(request.Id, cancellationToken)
        ).ToList();

        var roomImages = await _imageRepository.GetRoomImagesAsync(
            roomImagesIds,
            request.Page,
            request.PageSize - hotelImages.Count,
            cancellationToken
        );

        var images = roomImages.Concat(hotelImages);

        return images.Select(i => new ImageResponse(i.Url)).ToArray();
    }
}
