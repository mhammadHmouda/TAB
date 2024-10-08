﻿using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.UserManagement.Activation;

public class ActivateUserCommandHandler : ICommandHandler<ActivateUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ActivateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(
        ActivateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetAsync(
            user => user.ActivationCode.Value == request.Token,
            cancellationToken
        );

        if (user.HasNoValue)
        {
            return DomainErrors.User.UserNotFound;
        }

        if (user.Value.ActivationCode.ExpiresAtUtc < _dateTimeProvider.UtcNow)
        {
            return DomainErrors.User.ActivationCodeExpired;
        }

        var activationResult = user.Value.Activate();

        if (activationResult.IsFailure)
        {
            return activationResult.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
