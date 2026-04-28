namespace RoomManagementSystem.Models.Dtos;

public record RoomDto(
    string Name,
    BuildingCode BuildingCode,
    int Floor,
    uint Capacity,
    bool HasProjector,
    bool IsActive);