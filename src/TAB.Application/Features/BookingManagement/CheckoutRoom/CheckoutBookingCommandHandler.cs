using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Core.Interfaces.Payment;
using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Enums;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.CheckoutRoom;

public class CheckoutBookingCommandHandler
    : ICommandHandler<CheckoutBookingCommand, Result<Session>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentService _paymentService;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutBookingCommandHandler(
        IBookingRepository bookingRepository,
        IPaymentService paymentService,
        IUserContext userContext,
        IUnitOfWork unitOfWork
    )
    {
        _bookingRepository = bookingRepository;
        _paymentService = paymentService;
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

        var canCheckoutResult = booking.CanCheckout();

        if (canCheckoutResult.IsFailure)
        {
            return canCheckoutResult.Error;
        }

        var paymentResult = await CreatePaymentSessionAsync(
            command.PaymentMethod,
            command.BookingId,
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

    private async Task<Result<Session>> CreatePaymentSessionAsync(
        string paymentMethod,
        int bookingId,
        CancellationToken cancellationToken
    )
    {
        return paymentMethod.ToLower() switch
        {
            var method when method == PaymentMethod.Stripe.ToString().ToLower()
                => await _paymentService.CreateStripeSessionAsync(bookingId, cancellationToken),
            var method when method == PaymentMethod.PayPal.ToString().ToLower()
                => await _paymentService.CreatePayPalSessionAsync(bookingId, cancellationToken),
            _ => DomainErrors.General.InvalidPaymentMethod
        };
    }
}
