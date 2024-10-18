using Infrastructure.Domain.Rooms;

namespace Infrastructure.Domain.Tenants;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public ICollection<Room> Rooms { get; set; } = new List<Room>();

    public ICollection<Maintenance.Maintenance> Maintenances { get; set; } = new List<Maintenance.Maintenance>();
}