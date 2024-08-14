using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Core.Interfaces.Payment;
using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.CheckoutRoom;

public class CheckoutBookingCommandHandler
    : ICommandHandler<CheckoutBookingCommand, Result<Session>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentServiceFactory _paymentServiceFactory;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutBookingCommandHandler(
        IBookingRepository bookingRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IPaymentServiceFactory paymentServiceFactory
    )
    {
        _bookingRepository = bookingRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _paymentServiceFactory = paymentServiceFactory;
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

        var checkoutResult = booking.CanBeCheckedOutBy(_userContext.Id);

        if (checkoutResult.IsFailure)
        {
            return checkoutResult.Error;
        }

        var paymentServiceResult = _paymentServiceFactory.Create(command.PaymentMethod);

        if (paymentServiceResult.IsFailure)
        {
            return paymentServiceResult.Error;
        }

        var paymentResult = await paymentServiceResult.Value.CreateSessionAsync(
            booking.Id,
            cancellationToken
        );

        if (paymentResult.IsFailure)
        {
            return paymentResult.Error;
        }

        booking.AddSessionId(paymentResult.Value.SessionId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return paymentResult.Value;
    }
}
