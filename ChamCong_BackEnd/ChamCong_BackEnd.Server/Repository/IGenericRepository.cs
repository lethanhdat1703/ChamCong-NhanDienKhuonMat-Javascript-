namespace ChamCong_BackEnd.Server.Repository
{
    public interface IGenericRepository<T,TDto> where T : class where TDto : class
    {
        TDto Add(TDto dto);
        void Delete(int id);
        IEnumerable<TDto> GetAll();
        TDto GetById(int id);
        void Update(TDto dto);
    }
}
