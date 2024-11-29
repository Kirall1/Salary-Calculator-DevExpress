using SalaryCalculator.Models;
using SalaryCalculator.ViewModels.Base;

namespace SalaryCalculator.ViewModels
{
    public class AdditionToSalaryViewModel : ViewModel
    {
        private readonly int _id;
        public int Id { get => _id; }

        private decimal _standard;
        public decimal Standard
        {
            get => _standard;
            set => Set(ref _standard, value);
        }

        private decimal _addition;
        public decimal Addition
        {
            get => _addition;
            set => Set(ref _addition, value);
        }

        private int _salaryDetailId;
        public int SalaryDetailId
        {
            get => _salaryDetailId;
            set => Set(ref _salaryDetailId, value);
        }

        public AdditionToSalaryViewModel() { }

        public AdditionToSalaryViewModel(AdditionToSalary additionToSalary)
        {
            _id = additionToSalary.Id;
            _standard = additionToSalary.Standard;
            _addition = additionToSalary.Addition;
            _salaryDetailId = additionToSalary.SalaryDetail.Id;
        }

        public AdditionToSalaryViewModel(AdditionToSalaryViewModel additionToSalary)
        {
            _id = additionToSalary.Id;
            _standard = additionToSalary.Standard;
            _addition = additionToSalary.Addition;
            _salaryDetailId = additionToSalary.SalaryDetailId;
        }

        public AdditionToSalary ToAdditionToSalary()
        {
            return new AdditionToSalary()
            {
                Id = _id,
                Standard = _standard,
                Addition = _addition
            };
        }
    }
}
