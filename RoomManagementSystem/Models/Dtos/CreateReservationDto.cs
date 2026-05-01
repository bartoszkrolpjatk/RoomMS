using System.ComponentModel.DataAnnotations;

namespace RoomManagementSystem.Models.Dtos;

public record CreateReservationDto(
    [Required]
    long? RoomId,
    [Required]
    string? OrganizerName,
    [Required]
    string? Topic,
    [Required]
    DateOnly? Date,
    [Required]
    TimeOnly? StartTime,
    [Required]
    TimeOnly? EndTime,
    [Required]
    ReservationStatus? Status
);