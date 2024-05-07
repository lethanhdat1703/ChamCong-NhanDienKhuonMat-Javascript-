using AutoMapper;
using Backend.Data;
using Backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly NhandienDbContext _context;
        private readonly IMapper _mapper;

        public GenericRepository( NhandienDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return (int)entity.GetType().GetProperty("Id").GetValue(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task UpdateAsync(int id, TEntity entity)
        {
            var existingEntity = await _context.Set<TEntity>().FindAsync(id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
