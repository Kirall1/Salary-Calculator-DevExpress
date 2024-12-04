using DevExpress.Mvvm;
using SalaryCalculator.Infrastructure;
using SalaryCalculator.ViewModels.Base;
using SalaryCalculator.Views;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;

namespace SalaryCalculator.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                Set(ref _currentView, value);
            }
        }

        public ICommand ChangeLanguage {  get; }

        public MainViewModel(SalaryDetailView view)
        {
            CurrentView = view;
            ChangeLanguage = new DelegateCommand<string>(cc => OnChangeLanguageExecute(cc));
        }

        private void OnChangeLanguageExecute(string cultureCode)
        {
            var culture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            TranslationSource.Instance.CurrentCulture = culture;
        }
    }
}
