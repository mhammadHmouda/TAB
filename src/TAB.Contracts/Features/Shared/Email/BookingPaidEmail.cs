namespace TAB.Contracts.Features.Shared.Email;

public record BookingPaidEmail(
    int BookingId,
    string EmailTo,
    string Name,
    string HotelName,
    decimal TotalPrice,
    string Currency
);
