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
            CreateMap<UserForRegisterDto, User>();
            CreateMap<Document, DocumentForReturnDto>(); 
            CreateMap<DocumentForCreationDto, Document>();
            CreateMap<Document, DocumentForDetailedDto>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>();
            CreateMap<AppointmentForCreationDto, Appointment>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>();
                //.ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                //.ForMember(m => m.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url))

        }
    }
}