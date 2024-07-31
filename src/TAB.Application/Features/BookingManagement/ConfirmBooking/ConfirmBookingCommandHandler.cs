using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.ConfirmBooking;

public class ConfirmBookingCommandHandler : ICommandHandler<ConfirmBookingCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmBookingCommandHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork
    )
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ConfirmBookingCommand request,
        CancellationToken cancellationToken
    )
    {
        var maybeBooking = await _bookingRepository.GetByIdAsync(
            request.BookingId,
            cancellationToken
        );

        if (maybeBooking.HasNoValue)
        {
            return DomainErrors.Booking.NotFound;
        }

        var booking = maybeBooking.Value;

        var result = booking.Confirm();

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
