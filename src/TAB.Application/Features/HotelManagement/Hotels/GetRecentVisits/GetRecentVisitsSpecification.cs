using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Hotels.GetRecentVisits;

public class GetRecentVisitsSpecification : BaseSpecification<Booking>
{
    public GetRecentVisitsSpecification(int limit, int userId)
    {
        ApplyNoTracking();

        ApplyPaging(1, limit);

        AddInclude(b => b.Hotel);
        AddInclude($"{nameof(Booking.Hotel)}.{nameof(Hotel.City)}");

        AddOrderByDescending(b => b.CheckInDate);

        AddCriteria(b => b.UserId == userId);
    }
}
