using DevExpress.Mvvm;
using SalaryCalculator.Views;

namespace SalaryCalculator.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public object CurrentView { get; set; }

        public MainViewModel(SalaryDetailView view)
        {
            CurrentView = view;
        }
    }
}
