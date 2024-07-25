using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.ReviewManagement.DeleteReview;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.ReviewManagement.Entities;
using TAB.Domain.Features.ReviewManagement.Repositories;
using TAB.Domain.Features.UserManagement.Enums;

namespace Application.UnitTests.Features.ReviewManagement;

public class DeleteReviewCommandTests
{
    private readonly IReviewRepository _reviewRepositoryMock;
    private readonly IUserContext _userContextMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly DeleteReviewCommand Command = new(1);

    private static readonly Review Review = Review.Create("Title", "Content", 5, 1, 1);

    private readonly DeleteReviewCommandHandler _sut;

    public DeleteReviewCommandTests()
    {
        _reviewRepositoryMock = Substitute.For<IReviewRepository>();
        _userContextMock = Substitute.For<IUserContext>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new DeleteReviewCommandHandler(
            _reviewRepositoryMock,
            _userContextMock,
            _unitOfWorkMock
        );

        _userContextMock.Id.Returns(1);
        _reviewRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.From(Review));
    }

    [Fact]
    public async Task Handle_WhenUserIsNotOwner_ReturnsUnauthorized()
    {
        _reviewRepositoryMock
            .GetByIdAsync(Command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.From(Review));

        _userContextMock.Id.Returns(2);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.General.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenReviewNotFound_ReturnsNotFound()
    {
        _reviewRepositoryMock
            .GetByIdAsync(Command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Review.NotFound);
    }

    [Fact]
    public async Task Handle_WhenReviewFound_DeletesReview()
    {
        _userContextMock.Id.Returns(1);

        _reviewRepositoryMock
            .GetByIdAsync(Command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.From(Review));

        var result = await _sut.Handle(Command, CancellationToken.None);

        _reviewRepositoryMock.Received().Remove(Review);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenReviewFound_SavesChanges()
    {
        _userContextMock.Id.Returns(1);

        _reviewRepositoryMock
            .GetByIdAsync(Command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Review>.From(Review));

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await _unitOfWorkMock.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
