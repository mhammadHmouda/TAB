using Microsoft.Extensions.Options;
using Stripe.Checkout;
using TAB.Application.Core.Interfaces.Payment;
using TAB.Application.Features.BookingManagement.CancelBooking;
using TAB.Domain.Core.Enums;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Infrastructure.Payment.Options;
using Session = TAB.Contracts.Features.Shared.Session;

namespace TAB.Infrastructure.Payment;

public class StripeService : IPaymentService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly StripeOptions _stripeOptions;
    private readonly SessionService _sessionService;

    private const string DefaultImageUrl =
        "https://i.imgur.com/iw0UgTm_d.webp?maxwidth=1520&fidelity=grand";

    public StripeService(
        IOptions<StripeOptions> stripeOptions,
        SessionService sessionService,
        IBookingRepository bookingRepository
    )
    {
        _sessionService = sessionService;
        _bookingRepository = bookingRepository;
        _stripeOptions = stripeOptions.Value;
    }

    public async Task<Result<Session>> CreateSessionAsync(
        int bookingId,
        CancellationToken cancellation
    )
    {
        var bookingMaybe = await _bookingRepository.GetAsync(
            new BookingWithAllInfoSpecification(bookingId),
            cancellation
        );

        if (bookingMaybe.HasNoValue)
        {
            return DomainErrors.Booking.NotFound;
        }

        var booking = bookingMaybe.Value;

        var options = new SessionCreateOptions
        {
            SuccessUrl = _stripeOptions.SuccessUrl,
            CancelUrl = _stripeOptions.CancelUrl,
            PaymentMethodTypes = new List<string> { "card" },
            Mode = "payment",
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)booking.TotalPrice.Amount * 100,
                        Currency = booking.Room.Price.Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"{booking.Room.Type} Room - {booking.Room.Number}",
                            Description =
                                $"Book a {booking.Room.Type.ToString().ToLower()} room, room number {booking.Room.Number}, at our esteemed hotel.",
                            Images = new List<string> { DefaultImageUrl }
                        }
                    },
                    Quantity = 1
                }
            }
        };

        var session = await _sessionService.CreateAsync(options, cancellationToken: cancellation);

        return new Session(session.Id, session.Url);
    }

    public PaymentMethod GetPaymentMethod() => PaymentMethod.Stripe;
}
