using Application.Contracts;
using FluentValidation;

namespace Application.Validators
{
    public class ColivingRequestValidator : AbstractValidator<ColivingCreateDto>
    {
        public ColivingRequestValidator()
        {
            RuleFor(or=>or.Name).NotEmpty().MaximumLength(255).WithMessage("Coliving name is required.");
            
            RuleFor(or=>or.Description).NotEmpty().MaximumLength(255).WithMessage("Description is required.");

            RuleFor(or => or.Address).NotEmpty().MaximumLength(255).WithMessage("Address is required");
            
            RuleFor(or => or.Email).NotEmpty().MaximumLength(255).WithMessage("Email is required");
        }
    }
}