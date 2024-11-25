using SalaryCalculator.Models;

namespace SalaryCalculator.Data.Repositories.Impl
{
    public class SalaryDetailRepository : BaseRepository<SalaryDetail>, ISalaryDetailRepository
    {
        public SalaryDetailRepository(DatabaseContext context) : base(context) { }
    }
}
