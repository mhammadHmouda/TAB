using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Images.UploadImages;
using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Enums;
using TAB.Domain.Core.Errors;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace Application.UnitTests.Features.HotelManagement.Images;

public class UploadImagesTests
{
    private readonly IImageRepository _imageRepositoryMock;
    private readonly IUploadFileService _uploadFileServiceMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly UploadImagesCommand Command =
        new(
            1,
            ImageType.Hotel,
            new FileRequest[] { new("test", "test", new[] { byte.MaxValue, }) }
        );

    private readonly UploadImagesCommandHandler _sut;

    public UploadImagesTests()
    {
        _imageRepositoryMock = Substitute.For<IImageRepository>();
        _uploadFileServiceMock = Substitute.For<IUploadFileService>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new UploadImagesCommandHandler(
            _uploadFileServiceMock,
            _imageRepositoryMock,
            _unitOfWorkMock
        );
    }

    [Fact]
    public async Task Handle_WhenFilesMoreThan5_ReturnsTooManyFilesError()
    {
        // Arrange
        var files = new FileRequest[]
        {
            new("test", "", new[] { byte.MaxValue, }),
            new("test", "test", new[] { byte.MaxValue, }),
            new("test", "test", new[] { byte.MaxValue, }),
            new("test", "test", new[] { byte.MaxValue, }),
            new("test", "test", new[] { byte.MaxValue, }),
            new("test", "test", new[] { byte.MaxValue, }),
        };

        var command = Command with { Files = files };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Image.InvalidImageCount);
    }

    [Fact]
    public async Task Handle_WhenFilesAreNotImages_ReturnsInvalidFileTypeError()
    {
        // Arrange
        var files = new FileRequest[] { new("test", "", new[] { byte.MaxValue, }), };
        var command = Command with { Files = files };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Image.InvalidImageType);
    }

    [Fact]
    public async Task Handle_WhenFileIsMoreThan5MB_ReturnsFileIsTooLargeError()
    {
        // Arrange
        var files = new FileRequest[] { new("test", "image/png", new byte[5 * 1024 * 1024 + 1]), };

        var command = Command with { Files = files };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Image.InvalidImageSize);
    }

    [Fact]
    public async Task Handle_WhenFilesAreValid_ReturnsUrls()
    {
        // Arrange
        var files = new FileRequest[] { new("test", "image/png", new byte[5 * 1024 * 1024]), };

        var command = Command with { Files = files };

        var fileResponses = new FileResponse[] { new("test", "test") };

        _uploadFileServiceMock.UploadFilesAsync(Arg.Any<FileRequest[]>()).Returns(fileResponses);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Value.Urls.Should().BeEquivalentTo(fileResponses.Select(x => x.Url));
    }

    [Theory]
    [InlineData(ImageType.Hotel)]
    [InlineData(ImageType.City)]
    [InlineData(ImageType.Room)]
    public async Task Handle_WhenFilesAreValid_ReturnsSuccess(ImageType imageType)
    {
        // Arrange
        var files = new FileRequest[] { new("test", "image/png", new byte[5 * 1024 * 1024]), };

        var command = Command with { Files = files, ImageType = imageType };

        var fileResponses = new FileResponse[] { new("test", "test") };

        _uploadFileServiceMock.UploadFilesAsync(Arg.Any<FileRequest[]>()).Returns(fileResponses);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenFilesAreValid_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var files = new FileRequest[] { new("test", "image/png", new byte[5 * 1024 * 1024]), };

        var command = Command with { Files = files };

        var fileResponses = new FileResponse[] { new("test", "test") };

        _uploadFileServiceMock.UploadFilesAsync(Arg.Any<FileRequest[]>()).Returns(fileResponses);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenFilesAreValid_ShouldCallInsertRange()
    {
        // Arrange
        var files = new FileRequest[] { new("test", "image/png", new byte[5 * 1024 * 1024]), };

        var command = Command with { Files = files };

        var fileResponses = new FileResponse[] { new("test", "test") };

        _uploadFileServiceMock.UploadFilesAsync(Arg.Any<FileRequest[]>()).Returns(fileResponses);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        await _imageRepositoryMock.Received().AddRangeAsync(Arg.Any<IReadOnlyCollection<Image>>());
    }
}
