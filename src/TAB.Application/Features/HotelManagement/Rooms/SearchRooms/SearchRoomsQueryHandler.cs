using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Rooms.SearchRooms;

public class SearchRoomsQueryHandler
    : IQueryHandler<SearchRoomsQuery, Result<PagedList<RoomResponse>>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IAmenityRepository _amenityRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;

    public SearchRoomsQueryHandler(
        IRoomRepository roomRepository,
        IAmenityRepository amenityRepository,
        IImageRepository imageRepository,
        IMapper mapper
    )
    {
        _roomRepository = roomRepository;
        _amenityRepository = amenityRepository;
        _imageRepository = imageRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<RoomResponse>>> Handle(
        SearchRoomsQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new SearchRoomsSpecification(
            request.Page,
            request.PageSize,
            request.Filters,
            request.Sorting
        );

        var rooms = await _roomRepository.SearchRoomsAsync(spec, cancellationToken);

        var totalCount = await _roomRepository.CountAsync(spec.ForCounting());

        var response = _mapper.Map<RoomResponse[]>(rooms);

        foreach (var room in response)
        {
            var images = await _imageRepository.GetByRoomIdAsync(room.Id, cancellationToken);
            room.Images = _mapper.Map<ImageResponse[]>(images);

            var amenities = await _amenityRepository.GetByRoomIdAsync(room.Id, cancellationToken);
            room.Amenities = _mapper.Map<AmenityResponse[]>(amenities);
        }

        return PagedList<RoomResponse>.Create(
            response.ToList(),
            request.Page,
            request.PageSize,
            totalCount
        );
    }
}
