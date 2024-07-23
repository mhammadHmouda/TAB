using MediatR;
using TAB.Application.Core.Contracts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Amenities.DeleteAmenity;

public record DeleteAmenityCommand(int Id) : ICommand<Result>;
