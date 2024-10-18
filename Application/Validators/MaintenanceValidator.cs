using Application.Contracts;
using Application.Contracts.Maintenance;
using FluentValidation;

namespace Application.Validators
{
    public class MaintenanceRequestValidator : AbstractValidator<MaintenanceCreateDto>
    {
        public MaintenanceRequestValidator()
        {
            RuleFor(or=>or.Title).NotEmpty().MaximumLength(255).WithMessage("Title is required.");

            RuleFor(or => or.Description).NotEmpty().MaximumLength(255).WithMessage("Description is required");
        }
    }
}