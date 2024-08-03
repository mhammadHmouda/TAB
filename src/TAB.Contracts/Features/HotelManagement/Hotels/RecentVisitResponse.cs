using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Contracts.Features.HotelManagement.Hotels;

public record RecentVisitResponse(
    string HotelName,
    string City,
    string ThumbnailUrl,
    int Rating,
    Money Price,
    DateTime CheckInDate,
    DateTime CheckOutDate
);
