using Microsoft.EntityFrameworkCore;
using SalaryCalculator.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace SalaryCalculator.Data.Repositories.Impl
{
    public class AdditionToSalaryRepository : BaseRepository<AdditionToSalary>, IAdditionToSalaryRepository
    {
        public AdditionToSalaryRepository(DatabaseContext context) : base(context) { }

        public override async Task<IEnumerable<AdditionToSalary>> GetAllAsync()
        {
            return await _context.AdditionToSalaries.Include(a => a.SalaryDetail).ToListAsync();
        }

        public override async Task<AdditionToSalary> GetByIdAsync(int id)
        {
            return await _context.AdditionToSalaries.Include(a => a.SalaryDetail).FirstOrDefaultAsync(ats => ats.Id == id);
        }
    }
}
