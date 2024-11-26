using System.Threading.Tasks;
using System.Collections.Generic;
using SalaryCalculator.Models;

namespace SalaryCalculator.Data.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseModel
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        void Update(TEntity entitty);
        void Delete(TEntity entity);
    }
}
