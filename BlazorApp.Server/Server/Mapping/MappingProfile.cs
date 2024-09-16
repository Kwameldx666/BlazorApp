using AutoMapper;
using BlazorApp.Models;
using BlazorApp.Shared.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Настройка маппинга между UserData и UserDto
        CreateMap<UserData, UserDto>();
        CreateMap<UserDto, UserData>();
        CreateMap<Reservation, ReservationDto>();
        CreateMap<ReservationDto, Reservation>();
        CreateMap<UserData, UserDataDto>();
        CreateMap<UserDataDto, UserData>();
    }
}
