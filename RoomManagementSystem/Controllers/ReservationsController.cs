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
        if (!TimeValid(createReservationDto.Date, createReservationDto.StartTime, createReservationDto.EndTime, createReservationDto.RoomId, out var errorMessage) ||
            !RoomValid(createReservationDto.RoomId, out errorMessage))
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
        
        if (!TimeValid(updateReservationDto.Date, updateReservationDto.StartTime, updateReservationDto.EndTime, updateReservationDto.RoomId, out var errorMessage) ||
            !RoomValid(updateReservationDto.RoomId, out errorMessage))
            return BadRequest(errorMessage);

        mapper.Map(updateReservationDto, reservationById);
        return NoContent();
    }

    private bool RoomValid(long roomId, out string? errorMessage)
    {
        errorMessage = null;
        var roomById = roomRepository.Rooms.FirstOrDefault(r => r.Id == roomId);
        if (roomById == null)
        {
            errorMessage = "Room does not exist!";
            return false;
        }

        if (!roomById.IsActive)
        {
            errorMessage = "Room is inactive!";
            return false;
        }

        return true;
    }

    private bool TimeValid(DateOnly date, TimeOnly startTime, TimeOnly endTime, long roomId, out string? errorMessage)
    {
        errorMessage = null;
        if (endTime <= startTime)
        {
            errorMessage = "EndTime must be greater than StartTime";
            return false;
        }

        var conflictingReservation = reservationRepository.Reservations.FirstOrDefault(r => 
            r.Date == date && r.RoomId == roomId && startTime < r.EndTime && r.StartTime < endTime);
        if (conflictingReservation != null)
        {
            errorMessage =
                $"Date/Time conflict detected between new reservation and existing one id: {conflictingReservation.Id}";
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