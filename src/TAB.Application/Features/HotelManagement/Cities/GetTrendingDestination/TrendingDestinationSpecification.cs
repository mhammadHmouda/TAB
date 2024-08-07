using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Cities.GetTrendingDestination;

public class TrendingDestinationSpecification : BaseSpecification<Booking>
{
    public TrendingDestinationSpecification()
    {
        ApplyNoTracking();

        AddOrderBy(b => b.CheckInDate);

        AddInclude(x => x.Hotel);
        AddInclude($"{nameof(Booking.Hotel)}.{nameof(Hotel.City)}");
    }
}
