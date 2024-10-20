namespace Application.Contracts.Maintenance;

public class MaintenanceCreateDto
{
    public Guid? Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid RoomId { get; set; }
    public Guid TenantId { get; set; }
}