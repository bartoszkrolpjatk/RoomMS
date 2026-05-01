using System.ComponentModel.DataAnnotations;

namespace RoomManagementSystem.Models.Dtos;

public record CreateRoomDto
{
    [Required]
    [StringLength(20, MinimumLength = 1)]
    public required string Name { get; set; }

    [Required] 
    public required BuildingCode BuildingCode { get; set; }
    
    [Required] 
    public required int Floor { get; set; }
    
    [Required] [Range(1, 50)] 
    public required uint Capacity { get; set; }
    
    [Required] 
    public required bool HasProjector { get; set; }
    
    [Required] 
    public required bool IsActive { get; set; }
}