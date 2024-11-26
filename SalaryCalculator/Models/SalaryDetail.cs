using DevExpress.Drawing.TextFormatter.Internal;

namespace SalaryCalculator.Models
{
    public class SalaryDetail : BaseModel
    {
        public string Performer { get; set; }
        public virtual RankCoefficient RankCoefficient { get; set; }
        public decimal MonthlyBaseRate { get; set; }
        public decimal HourBaseRate { get; set; }
        public int HoursOfWorkPerDay {  get; set; }
        public int EffectiveWorkingTimeFund { get; set; }
        public decimal PremiumCoefficient { get; set; } = 1.2M;
        public decimal Salary { get; set; }

        private void CalculateMonthlyBaseRate()
        {
            MonthlyBaseRate = RankCoefficient.Coefficient * 253;
        }

        private void CalculateHourBaseRate()
        {
            HourBaseRate = RankCoefficient.Coefficient * 253 / 168;
        }

        private void CalculateSalary()
        {
            Salary = HourBaseRate * HoursOfWorkPerDay * EffectiveWorkingTimeFund * PremiumCoefficient;
        }

        public void RecalculateAll()
        {
            CalculateMonthlyBaseRate();
            CalculateHourBaseRate();
            CalculateSalary();
        }

    }
}
