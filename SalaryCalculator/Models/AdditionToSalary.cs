namespace SalaryCalculator.Models
{
    public class AdditionToSalary : BaseModel
    {
        public decimal Standard {  get; set; }
        public decimal Addition { get; set; }

        public virtual SalaryDetail SalaryDetail { get; set; }

        public void CalculateAddition()
        {
            Addition = SalaryDetail.Salary * Standard / 100;
        }
    }
}
