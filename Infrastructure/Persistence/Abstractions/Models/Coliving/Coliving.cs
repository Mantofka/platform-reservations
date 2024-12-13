using Infrastructure.Domain.Rooms;
using Infrastructure.Domain.User;

namespace Infrastructure.Persistence.Abstractions.Models.Coliving;

public class Coliving
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    
    public string Description { get; set; }
    
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    
    public Guid UserId { get; set; }
    
    public User User { get; set; }
}