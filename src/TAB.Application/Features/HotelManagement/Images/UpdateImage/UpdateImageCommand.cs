using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Images.UpdateImage;

public record UpdateImageCommand(int Id, FileRequest File) : ICommand<Result<UpdateImageResponse>>;
