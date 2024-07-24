using FluentValidation;

namespace TAB.Application.Features.HotelManagement.Rooms.AddRoom;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.HotelId).GreaterThan(0).WithMessage("The hotel id is required.");
        RuleFor(x => x.Number).GreaterThan(0).WithMessage("The room number is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("The room description is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("The room price is required.");
        RuleFor(x => x.Currency).NotEmpty().WithMessage("The room currency is required.");
        RuleFor(x => x.Type).IsInEnum().WithMessage("The room type is required.");
        RuleFor(x => x.CapacityOfAdults)
            .GreaterThan(0)
            .WithMessage("The room capacity of adults is required.");
        RuleFor(x => x.CapacityOfChildren)
            .GreaterThanOrEqualTo(0)
            .WithMessage("The room capacity of children is required.");
    }
}
