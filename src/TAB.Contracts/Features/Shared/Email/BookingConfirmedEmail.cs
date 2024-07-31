namespace TAB.Contracts.Features.Shared.Email;

public record BookingConfirmedEmail(
    int BookingId,
    string Name,
    string EmailTo,
    string HotelName,
    string CheckInDate,
    string CheckOutDate,
    decimal TotalPrice,
    string Currency
);
