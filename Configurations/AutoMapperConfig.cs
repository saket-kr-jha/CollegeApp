using AutoMapper;
using DotNetCore_New.Data;
using DotNetCore_New.Model;

namespace DotNetCore_New.Configurations
{
    public class AutoMapperConfig : Profile
    {
       public AutoMapperConfig() {
            CreateMap<StudentDTO, Student>().ReverseMap();
            CreateMap<RoleDTO, Role>().ReverseMap();


            //configuration for different property names
            //CreateMap<StudentDTO, Student>().ForMember(n => n.StudentId, opt => opt.MapFrom(x => x.Id)).ReverseMap();

            //configuration for ignoring the property names
            //CreateMap<StudentDTO, Student>().ForMember(n => n.StudentId, opt => opt.Ignore()).ReverseMap();

            // AddTransform() for adding the meaningful message when there is no data or returned as null or converting data
            //CreateMap<StudentDTO, Student>().ReverseMap().AddTransform<string>(n => n.IsNullOrEmpty(n)? "No data available" : n);

            //If the email is null
            //CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n=>n.StudentEmail, opt=> opt.MapFrom(n => string.IsNullOrEmpty(n.StudentEmail) ? "No Email Address Found" :n.StudentEmail));
        }
    }
}
