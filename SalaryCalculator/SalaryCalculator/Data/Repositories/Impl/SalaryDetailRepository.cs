using Microsoft.EntityFrameworkCore;
using SalaryCalculator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalaryCalculator.Data.Repositories.Impl
{
    public class SalaryDetailRepository : BaseRepository<SalaryDetail>, ISalaryDetailRepository
    {
        public SalaryDetailRepository(DatabaseContext context) : base(context) { }

        public override async Task<IEnumerable<SalaryDetail>> GetAllAsync()
        {
            return await _context.SalaryDetails.Include(a => a.Additions).ToListAsync();
        }

        public override async Task<SalaryDetail> GetByIdAsync(int id)
        {
            return await _context.SalaryDetails.Include(a => a.Additions).FirstOrDefaultAsync(ats => ats.Id == id);
        }


        public override void Delete(SalaryDetail entity)
        {
            if (entity.Additions != null)
            {
                foreach (var item in entity.Additions)
                {
                    _context.AdditionToSalaries.Remove(item);
                }
            }
            base.Delete(entity);
        }
    }
}
