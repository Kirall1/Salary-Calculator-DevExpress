using System.Collections.Generic;

namespace SalaryCalculator.Models
{
    public class RankCoefficient : BaseModel
    {
        public int Rank { get; set; } //Разряд
        public decimal Coefficient { get; set; } //Коэффициент
        public virtual IEnumerable<SalaryDetail> Details { get; set; }
    }
}
