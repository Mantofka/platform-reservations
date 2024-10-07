using Infrastructure.Domain.Rooms;

namespace Infrastructure.Persistence.Abstractions.Models.Coliving;

public class Coliving
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string RepresenterName { get; set; }

    public ICollection<Room> Rooms { get; set; } = new List<Room>();

}