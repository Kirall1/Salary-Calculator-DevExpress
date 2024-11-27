using System.Collections.Generic;

namespace SalaryCalculator.Models
{
    public class SalaryDetail : BaseModel
    {
        public string Performer { get; set; } //Испольнитель
        public virtual RankCoefficient RankCoefficient { get; set; } //Разряд и коэффициент
        public decimal MonthlyBaseRate { get; set; } //Месячная базова ставка
        public decimal HourBaseRate { get; set; } // Часовая базавая ставка
        public int HoursOfWorkPerDay {  get; set; } //Количество часов работы в день
        public int EffectiveWorkingTimeFund { get; set; } //Эффективный фонд рабочего времени
        public decimal PremiumCoefficient { get; set; } = 1.2M; //Коэффициент премирования 
        public decimal Salary { get; set; } //Зарплата
        public virtual IEnumerable<AdditionToSalary> Additions { get; set; }

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
            foreach (var item in Additions)
            {
                item.CalculateAddition();
            }
        }

    }
}
