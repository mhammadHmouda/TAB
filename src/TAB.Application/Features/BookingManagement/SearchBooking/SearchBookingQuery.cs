using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.BookingManagement;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.BookingManagement.SearchBooking;

public record SearchBookingQuery(int Page, int PageSize, string? Filters, string? Sorting)
    : IQuery<Result<PagedList<BookingResponse>>>;
