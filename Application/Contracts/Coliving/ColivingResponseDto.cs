using Application.Contracts.Room;

namespace Application.Contracts;

public class ColivingResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string RepresenterName { get; set; }
    
    public ICollection<RoomResponseDto> Rooms { get; set; }
}