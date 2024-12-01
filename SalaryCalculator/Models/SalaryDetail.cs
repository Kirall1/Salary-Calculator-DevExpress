using System.Collections.ObjectModel;

namespace SalaryCalculator.Models
{
    public class SalaryDetail : BaseModel
    {
        private string _performer;
        public string Performer
        {
            get => _performer;
            set => Set(ref _performer, value);
        }

        private RankCoefficient _rankCoefficient;
        public RankCoefficient RankCoefficient
        {
            get => _rankCoefficient;
            set => Set(ref _rankCoefficient, value);
        }

        private decimal _monthlyBaseRate;
        public decimal MonthlyBaseRate
        {
            get => _monthlyBaseRate;
            set => Set(ref _monthlyBaseRate, value);
        }

        private decimal _hourBaseRate;
        public decimal HourBaseRate
        {
            get => _hourBaseRate;
            set => Set(ref _hourBaseRate, value);
        }

        private int _hoursOfWorkPerDay;
        public int HoursOfWorkPerDay
        {
            get => _hoursOfWorkPerDay;
            set => Set(ref _hoursOfWorkPerDay, value);
        }

        private int _effectiveWorkingTimeFund;
        public int EffectiveWorkingTimeFund
        {
            get => _effectiveWorkingTimeFund;
            set => Set(ref _effectiveWorkingTimeFund, value);
        }

        private decimal _premiumCoefficient = 1.2M;
        public decimal PremiumCoefficient
        {
            get => _premiumCoefficient;
        }

        private decimal _salary;
        public decimal Salary
        {
            get => _salary;
            set => Set(ref _salary, value);
        }

        private ObservableCollection<AdditionToSalary> _additions;
        public ObservableCollection<AdditionToSalary> Additions
        {
            get => _additions;
            set => Set(ref _additions, value);
        }

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
            if (Additions != null)
            {
                foreach (var item in Additions)
                {
                    item.CalculateAddition();
                }
            }
        }

    }
}
