using TAB.Domain.Core.Primitives.Events;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.ReviewManagement.Events;

namespace TAB.Application.Features.ReviewManagement.UpdateReview;

public class ReviewUpdatedEventHandler : IDomainEventHandler<ReviewUpdatedEvent>
{
    private readonly IHotelRepository _hotelRepository;

    public ReviewUpdatedEventHandler(IHotelRepository hotelRepository) =>
        _hotelRepository = hotelRepository;

    public async Task Handle(ReviewUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var hotelMaybe = await _hotelRepository.GetByIdWithReviewsAsync(
            notification.HotelId,
            cancellationToken
        );

        if (hotelMaybe.HasNoValue)
        {
            return;
        }

        var hotel = hotelMaybe.Value;

        hotel.UpdateReview(notification.ReviewId);
    }
}
