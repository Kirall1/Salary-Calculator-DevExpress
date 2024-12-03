using SalaryCalculator.Models;
using System.Linq;

namespace SalaryCalculator.Data.Repositories.Impl
{
    public class RankCoefficientRepository : BaseRepository<RankCoefficient>, IRankCoefficientRepository
    {
        public RankCoefficientRepository(DatabaseContext context) : base(context) { }
    }
}
