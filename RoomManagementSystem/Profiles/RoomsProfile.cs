using AutoMapper;
using RoomManagementSystem.Models.Dtos;
using RoomManagementSystem.Models.Entities;

namespace RoomManagementSystem.Profiles;

public class RoomsProfile : Profile
{
    public RoomsProfile()
    {
        CreateMap<Room, RoomDto>();
        CreateMap<CreateRoomDto, Room>();
        CreateMap<UpdateRoomDto, Room>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}