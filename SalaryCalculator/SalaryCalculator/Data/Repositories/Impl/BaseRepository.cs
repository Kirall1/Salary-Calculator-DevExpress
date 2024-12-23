﻿using System.Threading.Tasks;
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
            await _context.SaveChangesAsync();
            return addedModel;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _dbSet.Where(o => o.Id == id).FirstOrDefaultAsync();

            return entity;
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            var e = _dbSet.Remove(entity);
            _context.SaveChanges();
        }

    }
}
