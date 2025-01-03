using AutoMapper;
using DotNetCore_New.Data;
using DotNetCore_New.Model;

namespace DotNetCore_New.Configurations
{
    public class AutoMapperConfig : Profile
    {
       public AutoMapperConfig() {
            CreateMap<StudentDTO, Student>().ReverseMap();
        }
    }
}
