using DevExpress.Xpf.Core;
using ImageCompressionResearchApp.Models;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImageCompressionResearchApp
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


        private void OriginalImageslListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is ImageModel selectedImage)
            {
                var viewModel = DataContext as MainViewModel;
                viewModel?.OpenOriginalImageCommand.Execute(selectedImage);
            }
        }

        private void CompressedImageslListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is ImageModel selectedImage)
            {
                var viewModel = DataContext as MainViewModel;
                viewModel?.OpenCompressedImageCommand.Execute(selectedImage);
            }
        }

        private void TextEdit_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorContent = "Значение не может быть пустым";
                return;
            }
            if (int.TryParse(e.Value.ToString(), out int value))
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
                e.ErrorContent = "Значение должно быть целым числом";
                return;
            }
        }
    }
}
