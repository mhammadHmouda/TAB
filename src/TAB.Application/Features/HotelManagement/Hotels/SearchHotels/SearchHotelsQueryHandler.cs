using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Hotels.Specifications;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Hotels.SearchHotels;

public class SearchHotelsQueryHandler
    : IQueryHandler<SearchHotelsQuery, Result<HotelSearchResponse[]>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SearchHotelsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<HotelSearchResponse[]>> Handle(
        SearchHotelsQuery request,
        CancellationToken cancellationToken
    )
    {
        var hotels = await _unitOfWork
            .Repository<Hotel>()
            .GetAllAsync(
                new HotelsSearchSpecification(
                    request.Filters,
                    request.Sorting,
                    request.Page,
                    request.PageSize
                ),
                cancellationToken
            );

        var hotelsResponse = _mapper.Map<HotelSearchResponse[]>(hotels);

        foreach (var hotel in hotelsResponse)
        {
            var images = await _unitOfWork
                .Repository<Image>()
                .GetAllAsync(i => i.ReferenceId == hotel.Id, cancellationToken);

            hotel.Images = _mapper.Map<ImageResponse[]>(images);

            var amenities = await _unitOfWork
                .Repository<Amenity>()
                .GetAllAsync(i => i.TypeId == hotel.Id, cancellationToken);

            hotel.Amenities = _mapper.Map<AmenityResponse[]>(amenities);
        }

        return hotelsResponse;
    }
}
