using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Images.UpdateImage;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Enums;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace Application.UnitTests.Features.HotelManagement;

public class UpdateImageCommandTests
{
    private readonly IImageRepository _imageRepositoryMock;
    private readonly IUploadFileService _uploadFileServiceMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly UpdateImageCommand Command =
        new(1, new FileRequest("test", "image/png", new[] { byte.MinValue, }));

    private static readonly Image ImageE = Image.Create("test", ImageType.City, 1).Value;

    private readonly UpdateImageCommandHandler _sut;

    public UpdateImageCommandTests()
    {
        _imageRepositoryMock = Substitute.For<IImageRepository>();
        _uploadFileServiceMock = Substitute.For<IUploadFileService>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new UpdateImageCommandHandler(
            _uploadFileServiceMock,
            _imageRepositoryMock,
            _unitOfWorkMock
        );
    }

    private void SetupMocksForValidFile()
    {
        _imageRepositoryMock
            .GetByIdAsync(Command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Image>.From(ImageE));

        _uploadFileServiceMock
            .UploadFilesAsync(Arg.Any<FileRequest[]>())
            .Returns(new[] { new FileResponse("test", "test") });
    }

    [Fact]
    public async Task Handle_WhenImageNotFound_ReturnsImageNotFoundError()
    {
        // Arrange
        _imageRepositoryMock
            .GetByIdAsync(Command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Image>.None);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Image.ImageNotFound);
    }

    [Fact]
    public async Task Handle_WhenFileIsNotImage_ReturnsInvalidImageTypeError()
    {
        // Arrange
        var command = Command with
        {
            File = new FileRequest("test", "application/pdf", new[] { byte.MinValue, })
        };

        _imageRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Image>.From(ImageE));

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Image.InvalidImageType);
    }

    [Fact]
    public async Task Handle_WhenFileIsValid_ReturnsSuccessResult()
    {
        // Arrange
        SetupMocksForValidFile();

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenFileIsValid_ShouldReturnUpdateImageResponse()
    {
        // Arrange
        SetupMocksForValidFile();

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Value.Should().BeOfType<UpdateImageResponse>();
    }

    [Fact]
    public async Task Handle_WhenFileIsValid_ShouldUpdateImageUrl()
    {
        // Arrange
        SetupMocksForValidFile();

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Value.Url.Should().Be("test");
    }

    [Fact]
    public async Task Handle_WhenFileIsValid_ShouldCallSaveChangesAsync()
    {
        // Arrange
        SetupMocksForValidFile();

        // Act
        await _sut.Handle(Command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
