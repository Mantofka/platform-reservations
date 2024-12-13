namespace Application.Contracts.Tenant;

public class TenantUpdateDto
{
    public Guid? Id { get; set; }
    
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    public DateTime BirthDate { get; set; }
}