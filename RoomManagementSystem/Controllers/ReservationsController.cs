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
            .Select(r =>
            {
                var reservationDto = mapper.Map<Reservation, ReservationDto>(r);
                var roomById = roomRepository.Rooms.First(room => room.Id == r.RoomId);
                reservationDto.Room = mapper.Map<Room, SimpleRoomDto>(roomById);
                return reservationDto;
            })
            .ToList();

        return Ok(result);
    }
}