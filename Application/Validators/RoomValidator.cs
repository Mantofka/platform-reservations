using Application.Contracts.Room;
using Application.Contracts.Tenant;
using FluentValidation;

namespace Application.Validators;

public class RoomValidator
{
    public class RoomCreateRequestValidator : AbstractValidator<RoomCreateDto>
    {
        public RoomCreateRequestValidator()
        {
            RuleFor(or=>or.Number).NotEmpty().WithMessage("Room number is required.");

            RuleFor(or => or.Size).NotEmpty().WithMessage("Room size is required");

            RuleFor(or => or.FloorNumber).NotEmpty().WithMessage("Room floor number is required");

            RuleFor(or => or.Description).NotEmpty().WithMessage("Room description is required");

            RuleFor(or => or.Price).NotEmpty().WithMessage("Room price is required");
        }
    }
    
    public class TenantAssignRequestValidator : AbstractValidator<AssignTenantDto>
    {
        public TenantAssignRequestValidator()
        {
            RuleFor(or=>or.TenantId).NotEmpty().WithMessage("Tenant id is required.");

            RuleFor(or => or.Roomid).NotEmpty().WithMessage("Room id is required.");
        }
    }
}