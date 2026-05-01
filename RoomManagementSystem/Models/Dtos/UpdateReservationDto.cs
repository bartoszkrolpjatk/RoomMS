using System.ComponentModel.DataAnnotations;

namespace RoomManagementSystem.Models.Dtos;

public record UpdateReservationDto
{
    [Required] public required long RoomId { get; set; }
    [Required] public required string OrganizerName { get; set; }
    [Required] public required string Topic { get; set; }
    [Required] public required DateOnly Date { get; set; }
    [Required] public required TimeOnly StartTime { get; set; }
    [Required] public required TimeOnly EndTime { get; set; }
    [Required] public required ReservationStatus Status { get; set; }
}