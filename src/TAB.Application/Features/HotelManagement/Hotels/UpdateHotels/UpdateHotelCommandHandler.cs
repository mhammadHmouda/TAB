using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Application.Features.HotelManagement.Hotels.UpdateHotels;

public class UpdateHotelCommandHandler : ICommandHandler<UpdateHotelCommand, Result>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHotelCommandHandler(
        IHotelRepository hotelRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result> Handle(
        UpdateHotelCommand request,
        CancellationToken cancellationToken
    )
    {
        var maybeHotel = await _hotelRepository.GetByIdAsync(request.Id, cancellationToken);

        if (maybeHotel.HasNoValue)
        {
            return DomainErrors.Hotel.NotFound;
        }

        var hotel = maybeHotel.Value;

        if (hotel.OwnerId != _userContext.Id)
        {
            return DomainErrors.General.Unauthorized;
        }

        var locationResult = Location.Create(request.Latitude, request.Longitude);

        if (locationResult.IsFailure)
        {
            return locationResult.Error;
        }

        var result = hotel.Update(request.Name, request.Description, locationResult.Value);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
