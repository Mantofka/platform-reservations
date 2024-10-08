namespace Application.Contracts;

public class ColivingCreateDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string RepresenterName { get; set; }
}