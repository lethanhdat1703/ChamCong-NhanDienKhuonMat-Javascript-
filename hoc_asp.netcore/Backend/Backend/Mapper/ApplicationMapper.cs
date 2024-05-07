using AutoMapper;
using Backend.DTO;
using Backend.Models;

namespace Backend.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<TimeSheet, TimeSheetDTO>().ReverseMap();
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<FaceModels,FaceModelsDTO>().ReverseMap();
        }
    }
}
