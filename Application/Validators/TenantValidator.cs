using Application.Contracts.Tenant;
using FluentValidation;

namespace Application.Validators
{
    public class TenantCreateRequestValidator : AbstractValidator<TenantCreateDto>
    {
        public TenantCreateRequestValidator()
        {
            RuleFor(or=>or.Name).NotEmpty().MaximumLength(255).WithMessage("Name is required.");

            RuleFor(or => or.Surname).NotEmpty().MaximumLength(255).WithMessage("Surname is required");

            RuleFor(or => or.BirthDate).NotEmpty().WithMessage("Birth date is required");

            RuleFor(or => or.Email).NotEmpty().WithMessage("Email is required").EmailAddress()
                .WithMessage("Provided address not having email format");

            RuleFor(or => or.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
        }
    }
}