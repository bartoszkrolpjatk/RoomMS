using RoomManagementSystem.Models.Entities;

namespace RoomManagementSystem.Models.Dtos;

public class RoomDto
{
    public string Name { get; set; }
    public BuildingCode BuildingCode { get; set; }
    public int Floor { get; set; }
    public int Capacity { get; set; }
    public bool HasProjektor { get; set; }
    public bool IsActive { get; set; }
}