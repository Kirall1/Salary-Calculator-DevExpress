using System.Threading.Tasks;
using System.Collections.Generic;
using SalaryCalculator.Models;

namespace SalaryCalculator.Data.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseModel
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> AddAsync(TEntity entity);
        TEntity Update(TEntity entitty);
        TEntity Delete(TEntity entity);
    }
}
