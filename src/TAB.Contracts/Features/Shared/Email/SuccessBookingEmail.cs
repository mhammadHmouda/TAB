using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Contracts.Features.Shared.Email;

public record BookingSuccessEmail(
    string Name,
    string EmailTo,
    string HotelName,
    string CheckInDate,
    string CheckOutDate,
    Money TotalPrice
);
