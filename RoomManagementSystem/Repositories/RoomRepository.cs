using RoomManagementSystem.Models.Entities;

namespace RoomManagementSystem.Repositories;

public class RoomRepository
{
    private readonly List<Room> _rooms =
    [
        new(1, "230", BuildingCode.B, 2, 15, true, true),
        new(2, "361", BuildingCode.A, 3, 30, true, true),
        new(3, "4", BuildingCode.B, -1, 8, true, true),
        new(4, "160", BuildingCode.A, 1, 20, false, false),
        new(5, "313", BuildingCode.H, 4, 18, false, true)
    ];

    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();
}