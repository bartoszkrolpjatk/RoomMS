using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoomManagementSystem.Models;
using RoomManagementSystem.Models.Dtos;
using RoomManagementSystem.Models.Entities;
using RoomManagementSystem.Repositories;

namespace RoomManagementSystem.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ReservationsController(
    IMapper mapper,
    ReservationRepository reservationRepository,
    RoomRepository roomRepository) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<ReservationDto>> GetReservations([FromQuery] DateOnly? date,
        [FromQuery] ReservationStatus? status, [FromQuery] long? roomId)
    {
        var result = reservationRepository.Reservations
            .Where(r => date == null || r.Date.Equals(date))
            .Where(r => status == null || r.Status == status)
            .Where(r => roomId == null || r.RoomId == roomId)
            .Select(MapToReservationDto)
            .ToList();

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public ActionResult<ReservationDto> GetReservationById([FromRoute] long id)
    {
        var reservationById = reservationRepository.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservationById == null)
            return NotFound();

        return MapToReservationDto(reservationById);
    }

    [HttpPost]
    public IActionResult CreateReservation([FromBody] CreateReservationDto createReservationDto)
    {
        if (CollisionDetected(createReservationDto.Date, createReservationDto.StartTime, createReservationDto.EndTime, createReservationDto.RoomId))
            return Conflict();
        if (!RoomExists(createReservationDto.RoomId, out var room))
            return NotFound("Room not found");
        if (!RequestValid(createReservationDto.StartTime, createReservationDto.EndTime, room, out var errorMessage))
            return BadRequest(errorMessage);

        var newReservation = mapper.Map<Reservation>(createReservationDto);
        newReservation.Id = reservationRepository.Reservations.Count + 1;
        reservationRepository.AddReservation(newReservation);
        return CreatedAtAction(nameof(GetReservationById), new { id = newReservation.Id }, newReservation);
    }

    [HttpPut("{id:long}")]
    public IActionResult UpdateReservation([FromRoute] long id, [FromBody] UpdateReservationDto updateReservationDto)
    {
        var reservationById = reservationRepository.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservationById == null)
            return NotFound();

        if (CollisionDetected(updateReservationDto.Date, updateReservationDto.StartTime, updateReservationDto.EndTime, updateReservationDto.RoomId))
            return Conflict();
        if (!RoomExists(updateReservationDto.RoomId, out var room))
            return NotFound("Room not found");
        if (!RequestValid(updateReservationDto.StartTime, updateReservationDto.EndTime, room, out var errorMessage))
            return BadRequest(errorMessage);

        mapper.Map(updateReservationDto, reservationById);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public IActionResult DeleteReservation([FromRoute] long id)
    {
        var reservationById = reservationRepository.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservationById == null)
            return NotFound();

        reservationRepository.RemoveReservation(reservationById);
        return NoContent();
    }

    private bool CollisionDetected(DateOnly date, TimeOnly startTime, TimeOnly endTime, long roomId)
    {
        return  reservationRepository.Reservations.Any(r =>
            r.Date == date && r.RoomId == roomId && startTime < r.EndTime && r.StartTime < endTime);
    }

    private bool RoomExists(long roomId, out Room? room)
    {
        room = roomRepository.Rooms.FirstOrDefault(r => r.Id == roomId);
        return room != null;
    }

    private bool RequestValid(TimeOnly startTime, TimeOnly endTime, Room room, out string? errorMessage)
    {
        errorMessage = null;
        if (!room.IsActive)
        {
            errorMessage = "Room is inactive!";
            return false;
        }
        if (endTime <= startTime)
        {
            errorMessage = "EndTime must be greater than StartTime";
            return false;
        }

        return true;
    }

    private ReservationDto MapToReservationDto(Reservation reservation)
    {
        var reservationDto = mapper.Map<Reservation, ReservationDto>(reservation);
        var roomById = roomRepository.Rooms.FirstOrDefault(room => room.Id == reservation.RoomId);
        if (roomById != null)
            reservationDto.Room = mapper.Map<Room, SimpleRoomDto>(roomById);
        return reservationDto;
    }
}