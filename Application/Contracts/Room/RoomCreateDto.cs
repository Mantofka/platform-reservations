namespace Application.Contracts.Room;

public class RoomCreateDto
{
    public Guid? Id { get; set; }
    public int Number { get; set; }
    public string Description { get; set; }
    public float Size { get; set; }
    public int FloorNumber { get; set; }
    public float Price { get; set; }
    
    public Guid ColivingId { get; set; }
}

