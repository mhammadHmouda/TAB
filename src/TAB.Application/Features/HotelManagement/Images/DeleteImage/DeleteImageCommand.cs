using TAB.Application.Core.Contracts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Images.DeleteImage;

public record DeleteImageCommand(int Id) : ICommand<Result>;
