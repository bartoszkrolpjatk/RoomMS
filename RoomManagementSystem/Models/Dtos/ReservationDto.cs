namespace RoomManagementSystem.Models.Dtos;

public class ReservationDto
{
    public SimpleRoomDto Room { get; set; }
    public string Topic { get; init; }
    public DateOnly Date { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public ReservationStatus Status { get; init; }
    
    public ReservationDto() { }
}