using System.Globalization;
using Microsoft.Extensions.Options;
using PayPal.Api;
using TAB.Application.Core.Interfaces.Payment;
using TAB.Application.Features.BookingManagement.CancelBooking;
using TAB.Domain.Core.Enums;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Infrastructure.Payment.Options;
using Session = TAB.Contracts.Features.Shared.Session;

namespace TAB.Infrastructure.Payment;

public class PayPalService : IPaymentService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly PayPalOptions _payPalOptions;

    public PayPalService(
        IOptions<PayPalOptions> payPalOptions,
        IBookingRepository bookingRepository
    )
    {
        _bookingRepository = bookingRepository;
        _payPalOptions = payPalOptions.Value;
    }

    public async Task<Result<Session>> CreateSessionAsync(
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
                        total = booking.TotalPrice.Amount.ToString(CultureInfo.InvariantCulture)
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

    public PaymentMethod GetPaymentMethod() => PaymentMethod.PayPal;
}
