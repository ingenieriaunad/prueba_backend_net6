using AutoMapper;
using WebApiAcademy.DTOs;
using WebApiAcademy.Models;
namespace WebApiAcademy.Utils
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Student, User>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol.Name))
            .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Person.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Person.Phone))
            .ForMember(dest => dest.CardId, opt => opt.MapFrom(src => src.Person.CardId));
        }
    }
}
