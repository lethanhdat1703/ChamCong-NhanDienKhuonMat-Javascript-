using AutoMapper;
using ChamCong_BackEnd.Server.Data;
using ChamCong_BackEnd.Server.Repository;
using Microsoft.EntityFrameworkCore;

namespace ChamCong_BackEnd.Server.Service
{
    public class GenericRepository<T, TDto> : IGenericRepository<T, TDto> where T : class where TDto : class
    {
        private readonly NhanDienDbContext _context;
        private readonly IMapper _mapper;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(NhanDienDbContext context, IMapper mapper, DbSet<T> dbSet)
        {
            _context = context;
            _mapper = mapper;
            _dbSet = dbSet;
        }

        public TDto Add(TDto dto)
        {
            var entity = _mapper.Map<T>(dto);
            _dbSet.Add(entity);
            _context.SaveChanges();
            return _mapper.Map<TDto>(entity);
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null) 
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }

        }

        public IEnumerable<TDto> GetAll()
        {
            return _dbSet.Select(e=>_mapper.Map<TDto>(e)).ToList();
        }

        public TDto GetById(int id)
        {
            var entity= _dbSet.Find(id);
            return _mapper.Map<TDto>(entity);
        }

        public void Update(TDto dto)
        {
            var entity = _mapper.Map<T>(dto);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
