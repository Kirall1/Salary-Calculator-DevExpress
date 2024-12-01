using SalaryCalculator.ViewModels.Base;

namespace SalaryCalculator.Models
{
    public abstract class BaseModel : ViewModel
    {
        public int Id { get; set; }
    }
}
