using TAB.Domain.Core.Primitives.Events;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.ReviewManagement.Events;

namespace TAB.Application.Features.ReviewManagement.DeleteReview;

public class ReviewDeletedEventHandler : IDomainEventHandler<ReviewDeletedEvent>
{
    private readonly IHotelRepository _hotelRepository;

    public ReviewDeletedEventHandler(IHotelRepository hotelRepository) =>
        _hotelRepository = hotelRepository;

    public async Task Handle(ReviewDeletedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.HotelId == default)
        {
            return;
        }

        var hotelMaybe = await _hotelRepository.GetByIdWithReviewsAsync(
            notification.HotelId,
            cancellationToken
        );

        if (hotelMaybe.HasNoValue)
        {
            return;
        }

        var hotel = hotelMaybe.Value;

        hotel.UpdateStarRating();
    }
}
