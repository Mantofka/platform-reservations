using Infrastructure.Domain.Rooms;

namespace Infrastructure.Domain.Tenants;

public class Tenant
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User.User User { get; set; }
    public ICollection<Room> Rooms { get; set; } = new List<Room>();

    public ICollection<Maintenance.Maintenance> Maintenances { get; set; } = new List<Maintenance.Maintenance>();
}
