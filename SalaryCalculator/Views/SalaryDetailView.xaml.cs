using SalaryCalculator.ViewModels;
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
