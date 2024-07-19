﻿using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.AddCity;

public record CreateCityCommand(string Name, string Country, string PostOffice)
    : ICommand<Result<CityResponse>>;
