using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Hotels.AddHotels;

public class CreateHotelCommandHandler : ICommandHandler<CreateHotelCommand, Result<HotelResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHotelCommandHandler(
        IHotelRepository hotelRepository,
        ICityRepository cityRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
    )
    {
        _hotelRepository = hotelRepository;
        _cityRepository = cityRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<HotelResponse>> Handle(
        CreateHotelCommand request,
        CancellationToken cancellationToken
    )
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId, cancellationToken);

        if (city.HasNoValue)
        {
            return DomainErrors.Hotel.CityNotFound;
        }

        var owner = await _userRepository.GetByIdAsync(request.OwnerId, cancellationToken);

        if (owner.HasNoValue)
        {
            return DomainErrors.Hotel.OwnerNotFound;
        }

        var locationResult = Location.Create(request.Latitude, request.Longitude);

        if (locationResult.IsFailure)
        {
            return locationResult.Error;
        }

        var hotel = Hotel.Create(
            request.Name,
            request.Description,
            locationResult.Value,
            request.Type,
            request.CityId,
            request.OwnerId
        );

        await _hotelRepository.InsertAsync(hotel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new HotelResponse(
            hotel.Id,
            hotel.Name,
            hotel.Description,
            hotel.Location.Latitude,
            hotel.Location.Longitude,
            city.Value.Name,
            city.Value.Country,
            hotel.Type.ToString(),
            $"{owner.Value.FirstName} {owner.Value.LastName}"
        );
    }
}
