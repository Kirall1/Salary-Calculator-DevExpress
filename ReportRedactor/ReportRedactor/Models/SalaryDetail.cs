using System.Collections.ObjectModel;

namespace ReportRedactor.Models
{
    public class SalaryDetail
    {
        public string Performer {  get; set; }

        public RankCoefficient RankCoefficient { get; set; }

        public decimal MonthlyBaseRate { get; set; }

        public decimal HourBaseRate { get; set; }

        public int HoursOfWorkPerDay { get; set; }

        public int EffectiveWorkingTimeFund {  get; set; }

        public decimal PremiumCoefficient { get; set; }

        public decimal Salary {  get; set; }

        public ObservableCollection<AdditionToSalary> Additions { get; set; }
    }
}
