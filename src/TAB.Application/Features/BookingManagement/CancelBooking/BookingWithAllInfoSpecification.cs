using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.BookingManagement.Entities;

namespace TAB.Application.Features.BookingManagement.CancelBooking;

public class BookingWithAllInfoSpecification : BaseSpecification<Booking>
{
    public BookingWithAllInfoSpecification(int bookingId)
    {
        ApplyNoTracking();

        AddInclude(b => b.User);
        AddInclude(b => b.Hotel);
        AddInclude(b => b.Room);

        AddCriteria(b => b.Id == bookingId);
    }
}
