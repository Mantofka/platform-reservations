namespace Application.Contracts.Tenant;

public class TenantCreateDto
{
    public Guid? Id { get; set; }
    public Guid UserId { get; set; }
}