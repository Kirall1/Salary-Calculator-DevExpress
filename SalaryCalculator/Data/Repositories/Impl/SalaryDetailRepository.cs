using SalaryCalculator.Models;

namespace SalaryCalculator.Data.Repositories.Impl
{
    public class SalaryDetailRepository : BaseRepository<SalaryDetail>, ISalaryDetailRepository
    {
        public SalaryDetailRepository(DatabaseContext context) : base(context) { }

        public override void Delete(SalaryDetail entity)
        {
            foreach (var item in entity.Additions)
            {
                _context.AdditionToSalaries.Remove(item);
            }
            base.Delete(entity);
        }
    }
}
