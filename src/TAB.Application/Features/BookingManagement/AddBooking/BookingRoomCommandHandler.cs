using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.BookingManagement;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.AddBooking;

public class BookingRoomCommandHandler
    : ICommandHandler<BookingRoomCommand, Result<BookingResponse>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookingRoomCommandHandler(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContext userContext
    )
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<Result<BookingResponse>> Handle(
        BookingRoomCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _userContext.Id;

        var userMaybe = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (userMaybe.HasNoValue)
        {
            return DomainErrors.User.UserNotFound;
        }

        var roomMaybe = await _roomRepository.GetByIdAsync(request.RoomId, cancellationToken);

        if (roomMaybe.HasNoValue)
        {
            return DomainErrors.Room.NotFound;
        }

        var room = roomMaybe.Value;

        if (!room.IsAvailable)
        {
            return DomainErrors.Room.NotAvailable;
        }

        var result = room.UpdateAvailability(false);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var hotelId = room.HotelId;

        var booking = Booking.Create(
            request.CheckInDate,
            request.CheckOutDate,
            userId,
            hotelId,
            request.RoomId,
            room.DiscountedPrice,
            room.Price.Currency
        );

        await _bookingRepository.AddAsync(booking);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BookingResponse>(booking);
    }
}
