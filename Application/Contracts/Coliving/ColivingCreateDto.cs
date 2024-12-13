namespace Application.Contracts;

public class ColivingCreateDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    
    public string Description { get; set; }
    
    public string Email { get; set; }
    
    public Guid? UserId { get; set; }
}