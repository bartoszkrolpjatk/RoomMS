using RoomManagementSystem.Models.Entities;

namespace RoomManagementSystem.Repositories;

public class RoomRepository
{
    public List<Room> Rooms { get; } =
    [
        new Room(1, "230", BuildingCode.B, 2, 15, true, true),
        new Room(2, "361", BuildingCode.A, 3, 30, true, true),
        new Room(3, "4", BuildingCode.B, -1, 8, true, true),
        new Room(4, "160", BuildingCode.A, 1, 20, false, false),
        new Room(5, "313", BuildingCode.H, 4, 18, false, true),
    ];
}