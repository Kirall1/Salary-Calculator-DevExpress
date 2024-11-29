using SalaryCalculator.Models;
using SalaryCalculator.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SalaryCalculator.ViewModels
{
    public class SalaryDetailViewModel : ViewModel
    {
        private readonly int _id;
        public int Id => _id;

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

        private ObservableCollection<AdditionToSalaryViewModel> _additions;
        public ObservableCollection<AdditionToSalaryViewModel> Additions
        {
            get => _additions;
            set => Set(ref _additions, value);
        }

        public SalaryDetailViewModel() { }

        public SalaryDetailViewModel(SalaryDetail salary)
        {
            _id = salary.Id;
            Performer = salary.Performer;
            RankCoefficient = salary.RankCoefficient;
            MonthlyBaseRate = salary.MonthlyBaseRate;
            HourBaseRate = salary.HourBaseRate;
            HoursOfWorkPerDay = salary.HoursOfWorkPerDay;
            EffectiveWorkingTimeFund = salary.EffectiveWorkingTimeFund;
            Salary = salary.Salary;
            Additions = new ObservableCollection<AdditionToSalaryViewModel>();
            if (salary.Additions == null)
            {
                return;
            }
            foreach (var addition in salary.Additions)
            {
                Additions.Add(new AdditionToSalaryViewModel(addition));
            }
        }

        public SalaryDetailViewModel(SalaryDetailViewModel salary)
        {
            _id = salary.Id;
            Performer = salary.Performer;
            RankCoefficient = salary.RankCoefficient;
            MonthlyBaseRate = salary.MonthlyBaseRate;
            HourBaseRate = salary.HourBaseRate;
            HoursOfWorkPerDay = salary.HoursOfWorkPerDay;
            EffectiveWorkingTimeFund = salary.EffectiveWorkingTimeFund;
            Salary = salary.Salary;
            Additions = salary.Additions;
        }

        public SalaryDetail ToSalaryDetail()
        {
            var salary = new SalaryDetail
            {
                Id = _id,
                Performer = Performer,
                RankCoefficient = RankCoefficient,
                MonthlyBaseRate = MonthlyBaseRate,
                HourBaseRate = HourBaseRate,
                HoursOfWorkPerDay = HoursOfWorkPerDay,
                EffectiveWorkingTimeFund = EffectiveWorkingTimeFund,
                Salary = Salary
            };
            return salary;
        }

        public void UpdateSalaryDetailViewModel(SalaryDetail salary)
        {
            Performer = salary.Performer;
            RankCoefficient = salary.RankCoefficient;
            MonthlyBaseRate = salary.MonthlyBaseRate;
            HourBaseRate = salary.HourBaseRate;
            HoursOfWorkPerDay = salary.HoursOfWorkPerDay;
            EffectiveWorkingTimeFund = salary.EffectiveWorkingTimeFund;
            Salary = salary.Salary;
            Additions = new ObservableCollection<AdditionToSalaryViewModel>();
            if (salary.Additions == null)
            {
                return;
            }
            foreach (var addition in salary.Additions)
            {
                Additions.Add(new AdditionToSalaryViewModel(addition));
            }
        }
    }
}
