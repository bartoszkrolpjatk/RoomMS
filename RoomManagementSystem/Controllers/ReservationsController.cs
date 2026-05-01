using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoomManagementSystem.Models;
using RoomManagementSystem.Models.Dtos;
using RoomManagementSystem.Models.Entities;
using RoomManagementSystem.Repositories;

namespace RoomManagementSystem.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ReservationsController(IMapper mapper, ReservationRepository reservationRepository, RoomRepository roomRepository) : ControllerBase
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
    public ActionResult<ReservationDto> GetReservations([FromRoute] long id)
    {
        var reservationById = reservationRepository.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservationById == null)
        {
            return NotFound();
        }

        return MapToReservationDto(reservationById);
    }

    private ReservationDto MapToReservationDto(Reservation reservation)
    {
        var reservationDto = mapper.Map<Reservation, ReservationDto>(reservation);
        var roomById = roomRepository.Rooms.First(room => room.Id == reservation.RoomId);
        reservationDto.Room = mapper.Map<Room, SimpleRoomDto>(roomById);
        return reservationDto;
    }
}