using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Core.Interfaces.Payment;
using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Enums;
using TAB.Domain.Features.BookingManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.CheckoutRoom;

public class CheckoutBookingCommandHandler
    : ICommandHandler<CheckoutBookingCommand, Result<Session>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ISessionService _sessionService;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutBookingCommandHandler(
        IBookingRepository bookingRepository,
        ISessionService sessionService,
        IUserContext userContext,
        IUnitOfWork unitOfWork
    )
    {
        _bookingRepository = bookingRepository;
        _sessionService = sessionService;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Session>> Handle(
        CheckoutBookingCommand command,
        CancellationToken cancellationToken
    )
    {
        var bookingMaybe = await _bookingRepository.GetByIdAsync(
            command.BookingId,
            cancellationToken
        );

        if (bookingMaybe.HasNoValue)
        {
            return DomainErrors.Booking.NotFound;
        }

        var booking = bookingMaybe.Value;

        if (booking.UserId != _userContext.Id)
        {
            return DomainErrors.General.Unauthorized;
        }

        if (booking.Status != BookingStatus.Confirmed)
        {
            return DomainErrors.Booking.NotConfirmed;
        }

        if (booking.Status == BookingStatus.Paid)
        {
            return DomainErrors.Booking.AlreadyPaid;
        }

        var sessionResult = await _sessionService.SaveAsync(command.BookingId, cancellationToken);

        if (sessionResult.IsFailure)
        {
            return sessionResult.Error;
        }

        booking.AddSessionId(sessionResult.Value.SessionId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return sessionResult.Value;
    }
}
