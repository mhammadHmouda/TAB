using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.ReviewManagement;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.ReviewManagement.Repositories;

namespace TAB.Application.Features.ReviewManagement.GetHotelReviews;

public class GetHotelReviewsQueryHandler
    : IQueryHandler<GetHotelReviewsQuery, Result<PagedList<ReviewResponse>>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelReviewsQueryHandler(
        IReviewRepository reviewRepository,
        IMapper mapper,
        IHotelRepository hotelRepository
    )
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
        _hotelRepository = hotelRepository;
    }

    public async Task<Result<PagedList<ReviewResponse>>> Handle(
        GetHotelReviewsQuery request,
        CancellationToken cancellationToken
    )
    {
        var hotelMaybe = await _hotelRepository.GetByIdAsync(request.HotelId, cancellationToken);

        if (hotelMaybe.HasNoValue)
        {
            return DomainErrors.Hotel.NotFound;
        }

        var spec = new ReviewsPaginatedAndOrderedSpecification(
            request.HotelId,
            request.Page,
            request.PageSize,
            request.Filters,
            request.Sorting
        );

        var reviews = await _reviewRepository.GetAllAsync(spec, cancellationToken);

        var totalCount = await _reviewRepository.CountAsync(spec.ForCounting());

        var mappedReviews = _mapper.Map<ReviewResponse[]>(reviews);

        return PagedList<ReviewResponse>.Create(
            mappedReviews.ToList(),
            request.Page,
            request.PageSize,
            totalCount
        );
    }
}
