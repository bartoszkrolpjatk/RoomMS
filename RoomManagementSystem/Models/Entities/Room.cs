namespace RoomManagementSystem.Models.Entities;

public class Room(
    long id,
    string name,
    BuildingCode buildingCode,
    int floor,
    uint capacity,
    bool hasProjector,
    bool isActive)
{
    public long Id { get; set; } = id;
    public string Name { get; set; } = name;
    public BuildingCode BuildingCode { get; set; } = buildingCode;
    public int Floor { get; set; } = floor;
    public uint Capacity { get; set; } = capacity;
    public bool HasProjector { get; set; } = hasProjector;
    public bool IsActive { get; set; } = isActive;
}