using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoomManagementSystem.Models.Dtos;
using RoomManagementSystem.Models.Entities;
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

    [HttpGet("{id:int}")]
    public ActionResult<RoomDto> GetRoomById([FromRoute] int id)
    {
        var result = roomRepository.Rooms.FirstOrDefault(r => r.Id == id);
        if (result == null)
            return NotFound();

        return mapper.Map<RoomDto>(result);
    }

    [HttpGet("{buildingCode}")]
    public ActionResult<List<RoomDto>> GetRoomsByBuildingCode([FromRoute] BuildingCode buildingCode)
    {
        return roomRepository.Rooms
            .Where(r => r.BuildingCode == buildingCode)
            .Select(mapper.Map<RoomDto>)
            .ToList();
    }
}