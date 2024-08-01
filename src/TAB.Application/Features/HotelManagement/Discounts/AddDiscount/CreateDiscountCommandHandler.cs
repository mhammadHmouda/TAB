using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.HotelManagement.Discounts;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Discounts.AddDiscount;

public class CreateDiscountCommandHandler
    : ICommandHandler<CreateDiscountCommand, Result<DiscountResponse>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDiscountCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DiscountResponse>> Handle(
        CreateDiscountCommand request,
        CancellationToken cancellationToken
    )
    {
        var roomMaybe = await _roomRepository.GetByIdWithDiscountsAsync(
            request.RoomId,
            cancellationToken
        );

        if (roomMaybe.HasNoValue)
        {
            return DomainErrors.Room.NotFound;
        }

        var room = roomMaybe.Value;

        var discount = Discount.Create(
            request.Name,
            request.Description,
            request.DiscountPercentage,
            request.StartDate,
            request.EndDate
        );

        var result = room.AddDiscount(discount);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DiscountResponse(
            discount.Id,
            discount.Name,
            discount.Description,
            discount.DiscountPercentage,
            discount.StartDate,
            discount.EndDate
        );
    }
}
