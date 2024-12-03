using DevExpress.Xpf.Core;
using ReportRedactor.ViewModels;

namespace ReportRedactor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            reportDesigner.ReportStorage = new ReportStorageViewModel(reportDesigner);
        }
    }
}
