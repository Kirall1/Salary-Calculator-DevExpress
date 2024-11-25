using System.Collections.Generic;

namespace SalaryCalculator.Models
{
    public class RankCoefficient : BaseModel
    {
        public int Rank { get; set; }
        public decimal Coefficient { get; set; }
        public virtual IEnumerable<SalaryDetail> Details { get; set; }
    }
}
