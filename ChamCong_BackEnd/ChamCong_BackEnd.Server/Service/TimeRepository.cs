using AutoMapper;
using ChamCong_BackEnd.Server.Data;
using ChamCong_BackEnd.Server.DTO;
using ChamCong_BackEnd.Server.Models;
using ChamCong_BackEnd.Server.Repository;

namespace ChamCong_BackEnd.Server.Service
{
    public class TimeRepository : ITimeRepository
    {
        private readonly NhanDienDbContext _context;
        private readonly IMapper _mapper;

        public TimeRepository(NhanDienDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public TimeDTO Add(TimeDTO timeDTO)
        {   
            var time = _mapper.Map<Time>(timeDTO);
            _context.Add(time);
            _context.SaveChanges();
            return _mapper.Map<TimeDTO>(time);
        }



        public void Delete(int id)
        {
            var time = _context.times.SingleOrDefault(t => t.Id == id);
            if (time != null)
            {
                _context.Remove(time);
                _context.SaveChanges();
            }
        }

        public List<TimeDTO> GetAll()
        {
            var times = _context.times.ToList();
            return _mapper.Map<List<TimeDTO>>(times);
        }

   

        public TimeDTO GetByID(int id)
        {
            var time = _context.times.SingleOrDefault(t => t.Id == id);
            if (time != null)
            {
                return _mapper.Map<TimeDTO>(time);
            }
            return null;
        }


        public void Update(TimeDTO timeDTO)
        {
            var time = _context.times.SingleOrDefault(t => t.Id == timeDTO.Id);
            if (time != null)
            {
                _mapper.Map(timeDTO, time);
                _context.SaveChanges();
            }
        }

    }
}
