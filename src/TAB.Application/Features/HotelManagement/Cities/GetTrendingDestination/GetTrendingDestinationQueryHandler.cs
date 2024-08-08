using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Cities;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Cities.GetTrendingDestination;

public class GetTrendingDestinationQueryHandler
    : IQueryHandler<GetTrendingDestinationQuery, Result<TrendingDestinationResponse[]>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IImageRepository _imageRepository;

    public GetTrendingDestinationQueryHandler(
        IBookingRepository bookingRepository,
        IImageRepository imageRepository
    )
    {
        _bookingRepository = bookingRepository;
        _imageRepository = imageRepository;
    }

    public async Task<Result<TrendingDestinationResponse[]>> Handle(
        GetTrendingDestinationQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new TrendingDestinationSpecification();
        var bookings = await _bookingRepository.GetAllAsync(spec, cancellationToken);

        var groupedBookings = bookings
            .GroupBy(b => b.Hotel.City)
            .Select(g => new
            {
                City = g.Key,
                Score = CalculateScore(g.Count(), g.Average(b => b.Hotel.StarRating))
            })
            .OrderByDescending(x => x.Score)
            .Take(request.Limit)
            .ToList();

        var cityIds = groupedBookings.Select(x => x.City.Id).ToList();
        var allThumbnails = await _imageRepository.GetAllByCityIdsAsync(cityIds, cancellationToken);

        return groupedBookings
            .Select(city => new TrendingDestinationResponse(
                city.City.Name,
                city.City.Country,
                allThumbnails.FirstOrDefault(x => x.ReferenceId == city.City.Id)?.Url
            ))
            .ToArray();
    }

    private static double CalculateScore(int visitCount, double averageStarRating) =>
        visitCount * 0.7 + averageStarRating * 0.3;
}
