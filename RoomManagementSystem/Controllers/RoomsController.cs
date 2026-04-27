using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoomManagementSystem.Models.Dtos;
using RoomManagementSystem.Repositories;

namespace RoomManagementSystem.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RoomsController(IMapper mapper, RoomRepository roomRepository) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<RoomDto>> GetAllRooms()
    {
        return roomRepository.Rooms
            .Select(mapper.Map<RoomDto>)
            .ToList();
    }
}