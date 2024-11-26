using SalaryCalculator.ViewModels.Base;
using SalaryCalculator.Views;

namespace SalaryCalculator.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public object CurrentView { get; set; }

        public MainViewModel(SalaryDetailView view)
        {
            CurrentView = view;
        }
    }
}
