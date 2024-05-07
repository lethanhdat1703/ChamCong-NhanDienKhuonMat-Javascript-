using AutoMapper;
using ChamCong_BackEnd.Server.DTO;
using ChamCong_BackEnd.Server.Models;

namespace ChamCong_BackEnd.Server.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Time, TimeDTO>();
            CreateMap<TimeDTO, Time>();
        }
    }
}
