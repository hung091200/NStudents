using AutoMapper;
using NStudents.Models.DTO;
using NStudents.Models.Entity;

namespace NStudents.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Students, StudentDto>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Classes.ClassName))
                .ForMember(dest => dest.MajorName, opt => opt.MapFrom(src => src.Classes.majors.MajorName));

            CreateMap<StudentCreateDto, Students>();
            CreateMap<StudentUpdateDto, Students>();
        }
    }
}
