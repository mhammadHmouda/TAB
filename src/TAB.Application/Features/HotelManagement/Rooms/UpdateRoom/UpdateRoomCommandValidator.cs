using FluentValidation;

namespace TAB.Application.Features.HotelManagement.Rooms.UpdateRoom;

public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Number).NotEmpty().WithMessage("Number is required");
        RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
        RuleFor(x => x.Currency).NotEmpty().WithMessage("Currency is required");
        RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
        RuleFor(x => x.CapacityOfAdults).NotEmpty().WithMessage("Capacity of adults is required");
        RuleFor(x => x.CapacityOfChildren)
            .NotEmpty()
            .WithMessage("Capacity of children is required");
    }
}
