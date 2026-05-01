using RoomManagementSystem.Models;
using RoomManagementSystem.Models.Entities;

namespace RoomManagementSystem.Repositories;

public class RoomRepository
{
    private readonly List<Room> _rooms =//todo: change to map
    [
        new()
        {
            Id = 1, Name = "230", BuildingCode = BuildingCode.B, Floor = 2, Capacity = 15, HasProjector = true, IsActive = true
        },
        new()
        {
            Id = 2, Name = "361", BuildingCode = BuildingCode.A, Floor = 3, Capacity = 30, HasProjector = true, IsActive = true
        },
        new()
        {
            Id = 3, Name = "4", BuildingCode = BuildingCode.B, Floor = -1, Capacity = 8, HasProjector = true, IsActive = true
        },
        new()
        {
            Id = 4, Name = "160", BuildingCode = BuildingCode.A, Floor = 1, Capacity = 20, HasProjector = false, IsActive = false
        },
        new()
        {
            Id = 5, Name = "313", BuildingCode = BuildingCode.H, Floor = 4, Capacity = 18, HasProjector = false, IsActive = true
        }
    ];

    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();

    public void AddRoom(Room room) => _rooms.Add(room);

    public void RemoveRoom(Room room) => _rooms.Remove(room);
}