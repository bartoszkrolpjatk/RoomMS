namespace RoomManagementSystem.Models.Entities;

public class Room
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required BuildingCode BuildingCode { get; set; }
    public required int Floor { get; set; }
    public required uint Capacity { get; set; }
    public required bool HasProjector { get; set; }
    public required bool IsActive { get; set; }

    public Room() { }
}