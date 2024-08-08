using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.ReviewManagement;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.ReviewManagement.Entities;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.ReviewManagement.AddReview;

public class CreateReviewCommandHandler
    : ICommandHandler<CreateReviewCommand, Result<ReviewResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateReviewCommandHandler(
        IHotelRepository hotelRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _hotelRepository = hotelRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ReviewResponse>> Handle(
        CreateReviewCommand request,
        CancellationToken cancellationToken
    )
    {
        var hotelMaybe = await _hotelRepository.GetByIdWithReviewsAsync(
            request.HotelId,
            cancellationToken
        );

        if (hotelMaybe.HasNoValue)
        {
            return DomainErrors.Hotel.NotFound;
        }

        var hotel = hotelMaybe.Value;

        var userMaybe = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (userMaybe.HasNoValue)
        {
            return DomainErrors.User.UserNotFound;
        }

        var review = Review.Create(
            request.Title,
            request.Content,
            request.Rating,
            request.HotelId,
            request.UserId
        );

        hotel.AddReview(review);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReviewResponse>(review);
    }
}
