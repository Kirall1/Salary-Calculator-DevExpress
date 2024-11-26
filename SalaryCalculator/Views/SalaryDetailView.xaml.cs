using DevExpress.Xpf.Grid;
using SalaryCalculator.Models;
using SalaryCalculator.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace SalaryCalculator.Views
{
    /// <summary>
    /// Interaction logic for SalaryDetailsView.xaml
    /// </summary>
    public partial class SalaryDetailView : UserControl
    {
        public SalaryDetailView(SalaryDetailViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
