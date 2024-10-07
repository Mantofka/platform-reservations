namespace Application.Contracts.Room;

public class AssignTenantDto
{
    public Guid TenantId { get; set; }
    public Guid Roomid { get; set; }
}