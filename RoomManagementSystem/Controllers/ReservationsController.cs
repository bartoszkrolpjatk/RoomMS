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
        if (!TimeValid(createReservationDto, out var errorMessage) ||
            !RoomValid(createReservationDto, out errorMessage))
            return BadRequest(errorMessage);

        var newReservation = mapper.Map<Reservation>(createReservationDto);
        newReservation.Id = reservationRepository.Reservations.Count + 1;
        reservationRepository.AddReservation(newReservation);
        return CreatedAtAction(nameof(GetReservationById), new { id = newReservation.Id }, newReservation);
    }

    private bool RoomValid(CreateReservationDto dto, out string? errorMessage)
    {
        errorMessage = null;
        var roomById = roomRepository.Rooms.FirstOrDefault(r => r.Id == dto.RoomId);
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

    private bool TimeValid(CreateReservationDto dto, out string? errorMessage)
    {
        errorMessage = null;
        if (dto.EndTime <= dto.StartTime)
        {
            errorMessage = "EndTime must be greater than StartTime";
            return false;
        }

        var conflictingReservation = reservationRepository.Reservations.FirstOrDefault(r => 
            r.Date == dto.Date && r.RoomId == dto.RoomId && dto.StartTime < r.EndTime && r.StartTime < dto.EndTime);
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