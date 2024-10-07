using Infrastructure.Domain.Tenants;
using Infrastructure.Persistence.Abstractions.Models.Coliving;

namespace Infrastructure.Domain.Rooms;

public class Room
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string Description { get; set; }
    public float Size { get; set; }
    public int FloorNumber { get; set; }
    public float Price { get; set; }
    public Guid ColivingId { get; set; }
    public Coliving Coliving { get; set; }
    
    public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
}