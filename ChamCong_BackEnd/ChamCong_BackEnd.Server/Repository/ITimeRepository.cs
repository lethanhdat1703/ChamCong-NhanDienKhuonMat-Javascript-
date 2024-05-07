using ChamCong_BackEnd.Server.DTO;
using ChamCong_BackEnd.Server.Models;

namespace ChamCong_BackEnd.Server.Repository
{
    public interface ITimeRepository
    {
        List<TimeDTO> GetAll();
        TimeDTO GetByID(int id);
        TimeDTO Add(TimeDTO timeDTO);
        void Update(TimeDTO timeDTO);
        void Delete(int id);
    }
}
