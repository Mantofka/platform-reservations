using Application.Contracts.Room;
using Application.Contracts.Tenant;

namespace Application.Contracts.Maintenance;

public class MaintenanceResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TenantResponseDto Tenant { get; set; }
    public RoomResponseDto Room { get; set; }
}