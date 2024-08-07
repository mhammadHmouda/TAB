using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.BookingManagement.Entities;

namespace TAB.Application.Features.BookingManagement.SearchBooking;

public class SearchBookingSpecification : BaseSpecification<Booking>
{
    public SearchBookingSpecification(int page, int pageSize, string? filters, string? sorting)
    {
        ApplyNoTracking();

        ApplyPaging(page, pageSize);
        AddDynamicFilters(filters);
        AddDynamicSorting(sorting);
    }
}
