using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using SalaryCalculator.Models;
using Res = SalaryCalculator.Properties.Resources;
using SalaryCalculator.ViewModels;
using System.Windows;
using System.Windows.Controls;
using SalaryCalculator.Infrastructure;

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
            if (e.Value == null || e.Value.ToString().Length == 0)
            {
                e.IsValid = false;
                e.ErrorContent = Res.PerformerRequirementMessage;
                return;
            }
            if (e.Value.ToString().Length >= 50) 
            {
                e.IsValid = false;
                e.ErrorContent = Res.PerformerMaxLenMessage;
                return;
            }
        }

        private void EffectiveWorkingTimeFundValidate(object sender, ValidationEventArgs e)
        {
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorContent = Res.IntegerNumberMessage;
                return;
            }
            if (int.TryParse(e.Value.ToString(), out int value))
            {
                if (value < 0 || value > 30)
                {
                    e.IsValid = false;
                    e.ErrorContent = $"{Res.ValueScopeMessage} 0 {Res.To} 30";
                    return;
                }
            }
            else
            {
                e.IsValid = false;
                e.ErrorContent = Res.IntegerNumberMessage;
                return;
            }
        }

        private void HoursOfWorkPerDayValidate(object sender, ValidationEventArgs e)
        {
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorContent = Res.IntegerNumberMessage;
                return;
            }
            if (int.TryParse(e.Value.ToString(), out int value))
            {
                if (value < 0 || value > 16)
                {
                    e.IsValid = false;
                    e.ErrorContent = $"{Res.ValueScopeMessage} 0 {Res.To} 16";
                    return;
                }
            }
            else
            {
                e.IsValid = false;
                e.ErrorContent = Res.IntegerNumberMessage;
                return;
            }
        }

        private void StandardValidate(object sender, ValidationEventArgs e)
        {
            if (e.Value == null || e.Value.ToString().Length == 0)
            {
                e.IsValid = false;
                e.ErrorContent = Res.NumberMessage;
                return;
            
            }
            if (double.TryParse(e.Value.ToString(), out double value))
            {
                if (value < 0 || value > 100)
                {
                    e.IsValid = false;
                    e.ErrorContent = $"{Res.ValueScopeMessage} 0 {Res.To} 100";
                    return;
                }
            }
            else
            {
                e.IsValid = false;
                e.ErrorContent = Res.NumberMessage;
                return;
            }
        }


        private void GridControl_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            var grid = (GridControl)e.Source;
            var masterGrid = grid.GetMasterGrid();
            var masterRowHandle = grid.GetMasterRowHandle();
            var masterSalaryDetail = masterGrid.GetRow(masterRowHandle) as SalaryDetail;
            var viewModel = (DataGridViewModel)((FrameworkElement)sender).DataContext;
            viewModel.SelectedAdditionSalaryDetail = masterSalaryDetail;
        }
    }
}
