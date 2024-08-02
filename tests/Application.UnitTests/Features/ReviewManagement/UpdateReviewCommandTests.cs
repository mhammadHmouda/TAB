using AutoMapper;
using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.ReviewManagement.UpdateReview;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.ReviewManagement.Entities;
using TAB.Domain.Features.ReviewManagement.Repositories;
using TAB.Domain.Features.UserManagement.Enums;

namespace Application.UnitTests.Features.ReviewManagement;

public class UpdateReviewCommandTests
{
    private readonly IReviewRepository _reviewRepositoryMock;
    private readonly IUserContext _userContextMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly UpdateReviewCommand Command = new(1, "Title", "Content", 5);

    private static readonly Review Review = Review.Create("Title", "Content", 5, 1, 1);

    private readonly UpdateReviewCommandHandler _sut;

    public UpdateReviewCommandTests()
    {
        _reviewRepositoryMock = Substitute.For<IReviewRepository>();
        _userContextMock = Substitute.For<IUserContext>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        var mapperMock = Substitute.For<IMapper>();

        _sut = new UpdateReviewCommandHandler(
            _reviewRepositoryMock,
            _userContextMock,
            _unitOfWorkMock,
            mapperMock
        );

        _userContextMock.Id.Returns(1);
        _reviewRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.From(Review));
    }

    [Fact]
    public async Task Handle_WhenUserUnauthorized_ReturnsUnauthorized()
    {
        _reviewRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Review);

        _userContextMock.Id.Returns(2);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.General.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenReviewNotFound_ReturnsNotFound()
    {
        _reviewRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Review.NotFound);
    }

    [Fact]
    public async Task Handle_WhenReviewFoundAndSame_ReturnsNothingToUpdate()
    {
        _userContextMock.Id.Returns(1);
        _userContextMock.Role.Returns(UserRole.User);

        _reviewRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Review);

        Review.Update(Command.Title, Command.Content, Command.Rating);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.Review.NothingToUpdate);
    }

    [Fact]
    public async Task Handle_WhenReviewFound_SavesChanges()
    {
        _userContextMock.Id.Returns(1);

        _reviewRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Review);

        Review.Update("New Title", "New Content", 5);

        var result = await _sut.Handle(Command, CancellationToken.None);

        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be(Command.Title);
        result.Value.Content.Should().Be(Command.Content);
        result.Value.Rating.Should().Be(Command.Rating);
    }
}
