using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Hotels.SearchHotels;

public class SearchHotelsQueryHandler
    : IQueryHandler<SearchHotelsQuery, Result<PagedList<HotelResponse>>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;

    public SearchHotelsQueryHandler(
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

    public async Task<Result<PagedList<HotelResponse>>> Handle(
        SearchHotelsQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new SearchHotelsSpecification(
            request.Filters,
            request.Sorting,
            request.Page,
            request.PageSize
        );

        var hotels = await _hotelRepository.SearchHotelsAsync(spec, cancellationToken);

        var totalCount = await _hotelRepository.CountAsync(spec.ForCounting());

        var hotelsResponse = _mapper.Map<HotelResponse[]>(hotels);

        foreach (var hotel in hotelsResponse)
        {
            var images = await _imageRepository.GetByHotelIdAsync(hotel.Id, cancellationToken);
            hotel.Images = _mapper.Map<ImageResponse[]>(images);

            var amenities = await _amenityRepository.GetByHotelIdAsync(hotel.Id, cancellationToken);
            hotel.Amenities = _mapper.Map<AmenityResponse[]>(amenities);
        }

        return PagedList<HotelResponse>.Create(
            hotelsResponse.ToList(),
            request.Page,
            request.PageSize,
            totalCount
        );
    }
}
