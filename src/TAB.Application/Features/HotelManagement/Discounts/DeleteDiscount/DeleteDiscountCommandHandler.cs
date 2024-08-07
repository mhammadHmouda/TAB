using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Discounts.DeleteDiscount;

public class DeleteDiscountCommandHandler : ICommandHandler<DeleteDiscountCommand, Result>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDiscountCommandHandler(
        IDiscountRepository discountRepository,
        IUnitOfWork unitOfWork
    )
    {
        _discountRepository = discountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteDiscountCommand request,
        CancellationToken cancellationToken
    )
    {
        var discountMaybe = await _discountRepository.GetByIdAsync(
            request.DiscountId,
            cancellationToken
        );

        if (discountMaybe.HasNoValue)
        {
            return DomainErrors.Discount.NotFound;
        }

        var discount = discountMaybe.Value;

        _discountRepository.Delete(discount);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
