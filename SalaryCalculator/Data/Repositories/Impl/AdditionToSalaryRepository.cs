using SalaryCalculator.Models;

namespace SalaryCalculator.Data.Repositories.Impl
{
    public class AdditionToSalaryRepository : BaseRepository<AdditionToSalary>, IAdditionToSalaryRepository
    {
        public AdditionToSalaryRepository(DatabaseContext context) : base(context) { }
    }
}
