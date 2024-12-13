using Application.Contracts.Room;

namespace Application.Contracts.Tenant;

public class TenantResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public List<RoomResponseDto> Rooms { get; set; }
}