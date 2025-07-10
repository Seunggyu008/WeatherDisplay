using AutoMapper;
using WeatherDisplay.Data;
using WeatherDisplay.Models;
using WeatherDisplay.Models.Profile;


namespace WeatherDisplay.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<ApplicationUser, ProfileVM>();
        }
    }
}
