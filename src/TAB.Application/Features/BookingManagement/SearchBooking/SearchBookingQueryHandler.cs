using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.BookingManagement;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.SearchBooking;

public class SearchBookingQueryHandler
    : IQueryHandler<SearchBookingQuery, Result<PagedList<BookingResponse>>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public SearchBookingQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<BookingResponse>>> Handle(
        SearchBookingQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new SearchBookingSpecification(
            request.Page,
            request.PageSize,
            request.Filters,
            request.Sorting
        );

        var bookings = await _bookingRepository.GetAllAsync(spec, cancellationToken);
        var totalCount = await _bookingRepository.CountAsync(spec.ForCounting());

        var bookingResponses = _mapper.Map<BookingResponse[]>(bookings);

        return PagedList<BookingResponse>.Create(
            bookingResponses.ToList(),
            request.Page,
            request.PageSize,
            totalCount
        );
    }
}
