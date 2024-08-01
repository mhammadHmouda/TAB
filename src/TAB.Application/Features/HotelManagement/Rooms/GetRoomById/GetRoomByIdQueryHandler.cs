using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Rooms.GetRoomById;

public class GetRoomByIdQueryHandler : IQueryHandler<GetRoomByIdQuery, Result<RoomSearchResponse>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IAmenityRepository _amenityRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;

    public GetRoomByIdQueryHandler(
        IRoomRepository roomRepository,
        IMapper mapper,
        IAmenityRepository amenityRepository,
        IImageRepository imageRepository
    )
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
        _amenityRepository = amenityRepository;
        _imageRepository = imageRepository;
    }

    public async Task<Result<RoomSearchResponse>> Handle(
        GetRoomByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var roomMaybe = await _roomRepository.GetByIdWithDiscountsAsync(
            request.Id,
            cancellationToken
        );

        if (roomMaybe.HasNoValue)
        {
            return DomainErrors.Room.NotFound;
        }

        var room = roomMaybe.Value;

        var roomResponse = _mapper.Map<RoomSearchResponse>(room);

        var amenities = await _amenityRepository.GetByRoomIdAsync(room.Id, cancellationToken);
        var images = await _imageRepository.GetByRoomIdAsync(room.Id, cancellationToken);

        return roomResponse with
        {
            Amenities = _mapper.Map<AmenityResponse[]>(amenities),
            Images = _mapper.Map<ImageResponse[]>(images)
        };
    }
}
