using AutoMapper;
using Telemedicine.API.Dtos;
using Telemedicine.API.Models;

namespace Telemedicine.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
            .ForMember(dest => dest.Age, opt => 
            opt.MapFrom(src => src.DateofBirth.CalculateAge()));
            CreateMap<User, UserForDetailedDto>()
            .ForMember(dest => dest.Age, opt => 
            opt.MapFrom(src => src.DateofBirth.CalculateAge()));
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotosForReturnDto>(); 
            CreateMap<PhotoForCreationDto, Photo>();
        }
    }
}