using DevExpress.Xpf.Editors;
using SalaryCalculator.ViewModels;
using System.Windows.Controls;

namespace SalaryCalculator.Views
{
    /// <summary>
    /// Interaction logic for SalaryDetailsView.xaml
    /// </summary>
    public partial class SalaryDetailView : UserControl
    {
        public SalaryDetailView(DataGridViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }


        private void PerformerValidate(object sender, ValidationEventArgs e)
        {
            if (e.Value == null) return;
            if (e.Value.ToString().Length == 0)
            {
                e.IsValid = false;
                e.ErrorContent = "Испольнитель обязателен";
                return;
            }
            if (e.Value.ToString().Length >= 50) 
            {
                e.IsValid = false;
                e.ErrorContent = "Испольнитель не может быть длиннее 50 символов";
                return;
            }
        }

        private void EffectiveWorkingTimeFundValidate(object sender, ValidationEventArgs e)
        {
            if (e.Value == null) return;
            if (int.TryParse(e.Value.ToString(), out int value))
            {
                if (value < 1 || value > 30)
                {
                    e.IsValid = false;
                    e.ErrorContent = "Значение должно быть в диапазоне от 1 до 30";
                    return;
                }
            }
            else
            {
                e.IsValid = false;
                e.ErrorContent = "Значение должно быть целым числом";
                return;
            }
        }

        private void HoursOfWorkPerDayValidate(object sender, ValidationEventArgs e)
        {
            if (e.Value == null) return; 
            if (int.TryParse(e.Value.ToString(), out int value))
            {
                if (value < 1 || value > 16)
                {
                    e.IsValid = false;
                    e.ErrorContent = "Значение должно быть в диапазоне от 0 до 16";
                    return;
                }
            }
            else
            {
                e.IsValid = false;
                e.ErrorContent = "Значение должно быть целым числом";
                return;
            }
        }

        private void StandardValidate(object sender, ValidationEventArgs e)
        {
            if (e.Value == null || e.Value.ToString().Length == 0) return;
            if (double.TryParse(e.Value.ToString(), out double value))
            {
                if (value < 0 || value > 100)
                {
                    e.IsValid = false;
                    e.ErrorContent = "Значение должно быть в диапазоне от 0 до 100";
                    return;
                }
            }
            else
            {
                e.IsValid = false;
                e.ErrorContent = "Значение должно быть числом";
                return;
            }
        }
    }
}
