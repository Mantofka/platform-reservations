using Infrastructure.Domain.Rooms;
using Infrastructure.Domain.Tenants;

namespace Infrastructure.Domain.RoomTenant;

public class RoomTenant
{
    public Guid RoomId { get; set; }
    public Room Room { get; set; }

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; }
}
