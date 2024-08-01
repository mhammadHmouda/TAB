using Microsoft.Extensions.Options;
using Stripe.Checkout;
using TAB.Application.Core.Interfaces.Payment;
using TAB.Application.Features.BookingManagement.CancelBooking;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Infrastructure.Payment.Options;
using Session = TAB.Contracts.Features.Shared.Session;

namespace TAB.Infrastructure.Payment;

public class StripeService : ISessionService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IImageRepository _imageRepository;
    private readonly StripeOptions _stripeOptions;
    private readonly SessionService _sessionService;

    private const string DefaultImageUrl =
        "https://i.imgur.com/iw0UgTm_d.webp?maxwidth=1520&fidelity=grand";

    public StripeService(
        IOptions<StripeOptions> stripeOptions,
        SessionService sessionService,
        IBookingRepository bookingRepository,
        IImageRepository imageRepository
    )
    {
        _sessionService = sessionService;
        _bookingRepository = bookingRepository;
        _imageRepository = imageRepository;
        _stripeOptions = stripeOptions.Value;
    }

    public async Task<Result<Session>> SaveAsync(int bookingId, CancellationToken cancellation)
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

        var images = await _imageRepository.GetByRoomIdAsync(
            bookingMaybe.Value.RoomId,
            cancellation
        );

        var imagesUrl = images.Select(i => i.Url).DefaultIfEmpty(DefaultImageUrl).ToList();

        var options = new SessionCreateOptions
        {
            SuccessUrl = _stripeOptions.SuccessUrl,
            CancelUrl = _stripeOptions.CancelUrl,
            PaymentMethodTypes = _stripeOptions.PaymentMethods,
            Mode = _stripeOptions.Mode,
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
                                Secure your stay in the exquisite {booking
                                    .Room
                                    .Type} Room at {booking.Hotel.Name},
                                nestled in the heart of {booking
                                    .Hotel
                                    .City
                                    .Name}. Your booking awaits in Room
                                {booking.Room.Number}—indulge in unparalleled comfort and style.
                                """,
                            Images = imagesUrl
                        }
                    },
                    Quantity = 1
                }
            }
        };

        var session = await _sessionService.CreateAsync(options, cancellationToken: cancellation);

        return new Session(session.Id, _stripeOptions.PublishableKey);
    }
}
