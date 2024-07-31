using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.CancelBooking;

public class CancelBookingCommandHandler : ICommandHandler<CancelBookingCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserContext _userContext;

    public CancelBookingCommandHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IUserContext userContext
    )
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _userContext = userContext;
    }

    public async Task<Result> Handle(
        CancelBookingCommand request,
        CancellationToken cancellationToken
    )
    {
        var bookingMaybe = await _bookingRepository.GetByIdAsync(
            request.BookingId,
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

        var result = booking.Cancel(_dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
