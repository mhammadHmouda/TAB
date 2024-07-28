using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Hotels.GetHotelById;

public class GetHotelByIdQueryHandler
    : IQueryHandler<GetHotelByIdQuery, Result<HotelSearchResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;

    public GetHotelByIdQueryHandler(
        IHotelRepository hotelRepository,
        IMapper mapper,
        IImageRepository imageRepository,
        IAmenityRepository amenityRepository
    )
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
        _imageRepository = imageRepository;
        _amenityRepository = amenityRepository;
    }

    public async Task<Result<HotelSearchResponse>> Handle(
        GetHotelByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new HotelSearchSpecification(request.Id);

        var hotelMaybe = await _hotelRepository.GetAsync(spec, cancellationToken);

        if (hotelMaybe.HasNoValue)
        {
            return DomainErrors.Hotel.NotFound;
        }

        var hotelResponse = _mapper.Map<HotelSearchResponse>(hotelMaybe.Value);

        var images = await _imageRepository.GetByHotelIdAsync(
            hotelMaybe.Value.Id,
            cancellationToken
        );
        hotelResponse.Images = _mapper.Map<ImageResponse[]>(images);

        var amenities = await _amenityRepository.GetByHotelIdAsync(
            hotelMaybe.Value.Id,
            cancellationToken
        );
        hotelResponse.Amenities = _mapper.Map<AmenityResponse[]>(amenities);

        return hotelResponse;
    }
}
