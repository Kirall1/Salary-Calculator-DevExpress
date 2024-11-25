using System.Threading.Tasks;
using SalaryCalculator.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace SalaryCalculator.Data.Repositories.Impl
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseModel
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var addedModel = (await _dbSet.AddAsync(entity)).Entity;

            return addedModel;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _dbSet.Where(o => o.Id == id).FirstOrDefaultAsync();

            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            _dbSet.Update(entity);

            return entity;
        }

        public TEntity Delete(TEntity entity)
        {
            return _dbSet.Remove(entity).Entity;
        }

    }
}
