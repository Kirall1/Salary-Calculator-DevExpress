using DevExpress.Xpf.Core;
using SqliteEncryptionApp.ViewModels;

namespace SqliteEncryptionApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
