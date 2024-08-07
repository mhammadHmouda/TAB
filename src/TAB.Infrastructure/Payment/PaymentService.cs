using System.Globalization;
using Microsoft.Extensions.Options;
using PayPal.Api;
using Stripe.Checkout;
using TAB.Application.Core.Interfaces.Payment;
using TAB.Application.Features.BookingManagement.CancelBooking;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Infrastructure.Payment.Options;
using Session = TAB.Contracts.Features.Shared.Session;

namespace TAB.Infrastructure.Payment;

public class PaymentService : IPaymentService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly StripeOptions _stripeOptions;
    private readonly PayPalOptions _payPalOptions;
    private readonly SessionService _sessionService;

    private const string DefaultImageUrl =
        "https://i.imgur.com/iw0UgTm_d.webp?maxwidth=1520&fidelity=grand";

    public PaymentService(
        IOptions<StripeOptions> stripeOptions,
        IOptions<PayPalOptions> payPalOptions,
        SessionService sessionService,
        IBookingRepository bookingRepository
    )
    {
        _sessionService = sessionService;
        _bookingRepository = bookingRepository;
        _stripeOptions = stripeOptions.Value;
        _payPalOptions = payPalOptions.Value;
    }

    public async Task<Result<Session>> CreateStripeSessionAsync(
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
                            Description = $"""
                                    Book a {booking
                                    .Room.Type.ToString()
                                    .ToLower()} room, room number {booking
                                    .Room
                                    .Number}, at our esteemed hotel.
                                    This room accommodates up to {booking
                                    .Room
                                    .AdultsCapacity} adults and {booking
                                    .Room
                                    .ChildrenCapacity} children,
                                    providing a comfortable and luxurious stay. The room features {booking
                                    .Room
                                    .Description},
                                    ensuring you have everything you need for a relaxing visit.
                                    Enjoy our exclusive discounts available for your booking period.
                                """,
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

    public async Task<Result<Session>> CreatePayPalSessionAsync(
        int bookingId,
        CancellationToken cancellationToken
    )
    {
        var bookingMaybe = await _bookingRepository.GetAsync(
            new BookingWithAllInfoSpecification(bookingId),
            cancellationToken
        );

        if (bookingMaybe.HasNoValue)
        {
            return DomainErrors.Booking.NotFound;
        }

        var booking = bookingMaybe.Value;

        var apiContext = new APIContext(
            new OAuthTokenCredential(
                _payPalOptions.ClientId,
                _payPalOptions.ClientSecret
            ).GetAccessToken()
        );

        var payment = new PayPal.Api.Payment
        {
            intent = "sale",
            payer = new Payer { payment_method = "paypal" },
            transactions = new List<Transaction>
            {
                new()
                {
                    description = $"{booking.Room.Type} Room - {booking.Room.Number}",
                    amount = new Amount
                    {
                        currency = booking.Room.Price.Currency,
                        total = (booking.TotalPrice.Amount).ToString(CultureInfo.InvariantCulture)
                    }
                }
            },
            redirect_urls = new RedirectUrls
            {
                return_url = _payPalOptions.SuccessUrl,
                cancel_url = _payPalOptions.CancelUrl
            }
        };

        var createdPayment = payment.Create(apiContext);

        return new Session(
            createdPayment.id,
            createdPayment.links.FirstOrDefault(x => x.rel == "approval_url")?.href!
        );
    }
}
