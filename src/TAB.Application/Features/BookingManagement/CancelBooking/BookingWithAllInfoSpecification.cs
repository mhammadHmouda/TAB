using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.BookingManagement.CancelBooking;

public class BookingWithAllInfoSpecification : BaseSpecification<Booking>
{
    public BookingWithAllInfoSpecification(int bookingId)
    {
        ApplyNoTracking();

        AddInclude(b => b.User);
        AddInclude(b => b.Hotel);
        AddInclude(b => b.Room);
        AddInclude($"{nameof(Booking.Hotel)}.{nameof(Hotel.City)}");

        AddCriteria(b => b.Id == bookingId);
    }
}
