using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.SuccessPayment;

public class SuccessPaymentCommandHandler : ICommandHandler<SuccessPaymentCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SuccessPaymentCommandHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork
    )
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        SuccessPaymentCommand request,
        CancellationToken cancellationToken
    )
    {
        var maybeBooking = await _bookingRepository.GetAsync(
            b => b.SessionId == request.SessionId,
            cancellationToken
        );

        if (maybeBooking.HasNoValue)
        {
            return DomainErrors.Booking.NotFound;
        }

        var booking = maybeBooking.Value;

        var result = booking.Pay();

        if (result.IsFailure)
        {
            return result.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
