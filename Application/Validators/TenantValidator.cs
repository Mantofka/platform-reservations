using Application.Contracts.Tenant;
using FluentValidation;

namespace Application.Validators
{
    public class TenantCreateRequestValidator : AbstractValidator<TenantCreateDto>
    {
        public TenantCreateRequestValidator()
        {
        }
    }
}