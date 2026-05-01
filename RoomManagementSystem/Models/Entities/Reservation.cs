namespace RoomManagementSystem.Models.Entities;

public class Reservation
{
    public required long Id { get; set; }
    public required long RoomId { get; set; }
    public required string OrganizerName { get; set; }
    public required string Topic { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
    public required ReservationStatus Status { get; set; }
    
    public Reservation() { }
}