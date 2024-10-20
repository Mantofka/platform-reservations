using Infrastructure.Domain.Rooms;
using Infrastructure.Domain.Tenants;

namespace Infrastructure.Domain.Maintenance;

public class Maintenance
{
    public Guid Id { get; set; }
    
    public Guid RoomId { get; set; }
    public Room AssignedRoom { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; }
}