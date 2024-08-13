using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Discounts;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Application.Features.HotelManagement.Hotels.GetFeaturedDeals;

public class GetFeaturedDealsQueryHandler
    : IQueryHandler<GetFeaturedDealsQuery, Result<List<FeaturedDealsResponse>>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GetFeaturedDealsQueryHandler(
        IHotelRepository hotelRepository,
        IImageRepository imageRepository,
        IMapper mapper,
        IDateTimeProvider dateTimeProvider
    )
    {
        _hotelRepository = hotelRepository;
        _imageRepository = imageRepository;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<List<FeaturedDealsResponse>>> Handle(
        GetFeaturedDealsQuery query,
        CancellationToken cancellationToken
    )
    {
        var hotels = await _hotelRepository.GetAllAsync(
            new GetFeaturedDealsSpecification(),
            cancellationToken
        );

        var featuredDeals = hotels
            .Select(hotel => new
            {
                Hotel = hotel,
                DiscountCount = hotel.Rooms.Sum(r => r.Discounts.Count),
                TotalPrice = hotel.Rooms.Average(r => r.Price.Amount)
            })
            .OrderBy(x => x.DiscountCount)
            .ThenByDescending(x => x.TotalPrice)
            .Take(query.Limit)
            .Select(x => CreateFeaturedDealsResponse(x.Hotel, cancellationToken))
            .ToList();

        return featuredDeals;
    }

    private FeaturedDealsResponse CreateFeaturedDealsResponse(
        Hotel hotel,
        CancellationToken cancellationToken
    )
    {
        var price = hotel.CalculateMinPrice();
        var discount = hotel.CalculateMaxDiscountPercentage();
        var discountedPrice = CalculateDiscountedPrice(price, discount);

        var roomDeals = hotel.Rooms.Select(CreateRoomDeal).ToList();

        var thumbnailUrl = _imageRepository
            .GetByHotelIdAsync(hotel.Id, cancellationToken)
            .Result.FirstOrDefault()
            ?.Url;

        return new FeaturedDealsResponse(
            hotel.Id,
            hotel.Name,
            thumbnailUrl,
            hotel.Location,
            price,
            discountedPrice,
            roomDeals
        );
    }

    private static int? CalculateDiscountedPrice(Money? price, int? discount)
    {
        if (price?.Amount == null || discount == null)
        {
            return null;
        }

        var discountedAmount = price.Amount - price.Amount * discount.Value / 100;
        return (int?)discountedAmount;
    }

    private RoomDeal CreateRoomDeal(Room room) =>
        new(
            room.Id,
            room.Number,
            room.Type.ToString(),
            room.Price,
            room.CalculatePrice(_dateTimeProvider.UtcNow),
            _mapper.Map<List<DiscountResponse>>(room.Discounts.ToList())
        );
}
