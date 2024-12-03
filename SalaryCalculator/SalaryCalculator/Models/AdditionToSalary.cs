namespace SalaryCalculator.Models
{
    public class AdditionToSalary : BaseModel
    {
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
        
        private SalaryDetail _salaryDetail;
        public SalaryDetail SalaryDetail
        {
            get => _salaryDetail;
            set => Set(ref _salaryDetail, value);
        
        }

        public void CalculateAddition()
        {
            Addition = SalaryDetail.Salary * Standard / 100;
        }
    }
}
