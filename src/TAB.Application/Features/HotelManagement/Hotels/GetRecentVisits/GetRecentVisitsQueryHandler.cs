using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Hotels.GetRecentVisits;

public class GetRecentVisitsQueryHandler
    : IQueryHandler<GetRecentVisitsQuery, Result<List<RecentVisitResponse>>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IUserContext _userContext;

    public GetRecentVisitsQueryHandler(
        IUserContext userContext,
        IBookingRepository bookingRepository,
        IImageRepository imageRepository
    )
    {
        _userContext = userContext;
        _bookingRepository = bookingRepository;
        _imageRepository = imageRepository;
    }

    public async Task<Result<List<RecentVisitResponse>>> Handle(
        GetRecentVisitsQuery query,
        CancellationToken cancellationToken
    )
    {
        var recentVisits = await _bookingRepository.GetAllAsync(
            new GetRecentVisitsSpecification(query.Limit, _userContext.Id),
            cancellationToken
        );

        var response = recentVisits.Select(CreateRecentVisitResponse).ToList();

        return response;
    }

    private RecentVisitResponse CreateRecentVisitResponse(Booking booking)
    {
        var thumbnailUrl = _imageRepository
            .GetByHotelIdAsync(booking.Hotel.Id, CancellationToken.None)
            .Result.FirstOrDefault()
            ?.Url;

        return new RecentVisitResponse(
            booking.Hotel.Name,
            booking.Hotel.City.Name,
            thumbnailUrl!,
            booking.Hotel.StarRating,
            booking.TotalPrice,
            booking.CheckInDate,
            booking.CheckOutDate
        );
    }
}
