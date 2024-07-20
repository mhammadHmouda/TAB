using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.HotelManagement.Cities;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.AddCity;

public class CreateCityCommandHandler : ICommandHandler<CreateCityCommand, Result<CityResponse>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCityCommandHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CityResponse>> Handle(
        CreateCityCommand request,
        CancellationToken cancellationToken
    )
    {
        var cityResult = City.Create(request.Name, request.Country, request.PostOffice);

        if (cityResult.IsFailure)
        {
            return cityResult.Error;
        }

        await _cityRepository.InsertAsync(cityResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var city = cityResult.Value;

        return new CityResponse(
            city.Id,
            city.Name,
            city.Country,
            city.PostOffice,
            city.CreatedAtUtc,
            city.UpdatedAtUtc
        );
    }
}
