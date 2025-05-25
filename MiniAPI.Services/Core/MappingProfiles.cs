using System;
using AutoMapper;
using MiniAPI.Models;

namespace MiniAPI.Services;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Game, Game>();
        CreateMap<GameUpdateDTO, Game>();
        CreateMap<RegisterDTO, User>();
    }
}