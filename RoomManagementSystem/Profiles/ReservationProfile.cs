using AutoMapper;
using RoomManagementSystem.Models.Dtos;
using RoomManagementSystem.Models.Entities;

namespace RoomManagementSystem.Profiles;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<Reservation, ReservationDto>();
        CreateMap<CreateReservationDto, Reservation>();
        CreateMap<UpdateReservationDto, Reservation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}