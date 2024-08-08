using AutoMapper;
using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Cities;
using TAB.Application.Features.HotelManagement.Cities.AddCity;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace Application.UnitTests.Features.HotelManagement.Cities;

public class CreateCityCommandTests
{
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly ICityRepository _cityRepositoryMock;
    private readonly CreateCityCommandHandler _sut;
    private readonly CreateCityCommand _command;

    public CreateCityCommandTests()
    {
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _cityRepositoryMock = Substitute.For<ICityRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CityProfile());
        });

        var mapper = config.CreateMapper();

        _sut = new CreateCityCommandHandler(_cityRepositoryMock, _unitOfWorkMock, mapper);

        _command = new CreateCityCommand("City", "Country", "PostOffice");
    }

    [Fact]
    public async Task Handle_Should_CreateCity()
    {
        // Arrange
        var city = City.Create("City", "Country", "PostOffice").Value;
        _cityRepositoryMock.AddAsync(city).Returns(Task.FromResult(city));

        // Act
        var result = await _sut.Handle(_command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("City");
        result.Value.Country.Should().Be("Country");
        result.Value.PostOffice.Should().Be("PostOffice");
    }

    [Fact]
    public async Task Handle_Should_CallUnitOfWorkSaveChangesAsync()
    {
        // Arrange
        var city = City.Create("City", "Country", "PostOffice").Value;
        _cityRepositoryMock.AddAsync(city).Returns(Task.FromResult(city));

        // Act
        await _sut.Handle(_command, default);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
