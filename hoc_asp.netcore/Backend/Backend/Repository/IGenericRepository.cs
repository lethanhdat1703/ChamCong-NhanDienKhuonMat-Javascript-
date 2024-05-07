namespace Backend.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<int> AddAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task UpdateAsync(int id, TEntity entity);

    }
}
