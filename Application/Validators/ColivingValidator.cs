using Application.Contracts;
using FluentValidation;

namespace Application.Validators
{
    public class ColivingRequestValidator : AbstractValidator<ColivingCreateDto>
    {
        public ColivingRequestValidator()
        {
            RuleFor(or=>or.Name).NotEmpty().MaximumLength(255).WithMessage("Coliving name is required.");

            RuleFor(or => or.Address).NotEmpty().MaximumLength(255).WithMessage("Address is required");

            RuleFor(or => or.RepresenterName).NotEmpty().MaximumLength(255).WithMessage("Representer Name is required");

            RuleFor(or => or.Email).NotEmpty().WithMessage("Email is required").EmailAddress()
                .WithMessage("Provided address not having email sturcture");

            RuleFor(or => or.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
        }
    }
}