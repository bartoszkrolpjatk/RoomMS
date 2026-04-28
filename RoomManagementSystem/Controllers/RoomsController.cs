using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoomManagementSystem.Models;
using RoomManagementSystem.Models.Dtos;
using RoomManagementSystem.Models.Entities;
using RoomManagementSystem.Repositories;

namespace RoomManagementSystem.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RoomsController(IMapper mapper, RoomRepository roomRepository) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<RoomDto>> GetRooms([FromQuery] uint? minCapacity, [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var result = roomRepository.Rooms
            .Where(r => minCapacity == null || r.Capacity >= minCapacity.Value)
            .Where(r => hasProjector == null || r.HasProjector == hasProjector.Value)
            .Where(r => activeOnly == null || !activeOnly.Value || r.IsActive)
            .Select(mapper.Map<RoomDto>)
            .ToList();
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public ActionResult<RoomDto> GetRoomById([FromRoute] long id)
    {
        var roomById = roomRepository.Rooms.FirstOrDefault(r => r.Id == id);
        if (roomById == null)
            return NotFound();

        return Ok(mapper.Map<RoomDto>(roomById));
    }

    [HttpGet("{buildingCode}")]
    public ActionResult<List<RoomDto>> GetRoomsByBuildingCode([FromRoute] BuildingCode buildingCode)
    {
        var result = roomRepository.Rooms
            .Where(r => r.BuildingCode == buildingCode)
            .Select(mapper.Map<RoomDto>)
            .ToList();
        return Ok(result);
    }

    [HttpPost]
    public IActionResult CreateRoom([FromBody] CreateRoomDto createRoomDto)
    {
        var newId = roomRepository.Rooms.Count + 1;
        var newRoom = mapper.Map<Room>(createRoomDto);
        newRoom.Id = newId;
        return CreatedAtAction(nameof(GetRoomById), new { id = newRoom.Id }, newRoom);
    }
}