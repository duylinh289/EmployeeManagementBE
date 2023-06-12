using AutoMapper;
using EmployeeManagementBE.Data;
using EmployeeManagementBE.DTO.Employee;
using EmployeeManagementBE.DTO.Student;
using EmployeeManagementBE.DTO.Task;
using EmployeeManagementBE.DTO.User;
using RepositoryCodeFirstCore.Data;
using System.ComponentModel.DataAnnotations;

namespace RepositoryCodeFirstCore.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Student, StudentsDTO>().ReverseMap();
            CreateMap<User, UserRegisterDTO>().ReverseMap();   
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
            CreateMap<TaskList, TaskAssignDTO>().ReverseMap();
            CreateMap<EmployeeImportTmp, ImportEmployeeDTO>().ReverseMap();
            CreateMap<Subject, SubjectDTO>().ReverseMap();
            //CreateMap<Class, TaskAssignDTO>().ReverseMap();
        }
    }
}
