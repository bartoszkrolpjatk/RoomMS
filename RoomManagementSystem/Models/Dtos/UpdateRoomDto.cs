using System.ComponentModel.DataAnnotations;

namespace RoomManagementSystem.Models.Dtos;

public record UpdateRoomDto(//todo: remove nullable types
    [Required]
    [StringLength(20, MinimumLength = 1)]
    string? Name,
    [Required]
    BuildingCode? BuildingCode,
    [Required]
    int? Floor,
    [Required]
    [Range(1, 50)]
    uint? Capacity,
    [Required]
    bool? HasProjector,
    [Required]
    bool? IsActive);