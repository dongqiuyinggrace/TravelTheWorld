using AutoMapper;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TripViewModel, Trip>().ReverseMap();
            CreateMap<StopViewModel, Stop>().ReverseMap();
        }
    }
}