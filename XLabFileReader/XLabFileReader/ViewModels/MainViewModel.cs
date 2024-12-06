using DevExpress.Mvvm;
using DevExpress.XtraPrinting.Native.Extensions;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using XLabFileReader.Models;
using XLabFileReader.Services.Interfaces;

namespace XLabFileReader.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IXLabFileService _fileService;

        private ObservableCollection<XLabFile> _files;
        public ObservableCollection<XLabFile> Files
        {
            get => _files;
            set => Set(ref _files, value);
        }

        private XLabFile _selectedFile;
        public XLabFile SelectedFile
        {
            get => _selectedFile;
            set => Set(ref _selectedFile, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        public ICommand LoadFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand DeleteFileCommand { get; }
        public ICommand SelectFileCommand { get; }
        public MainViewModel(IAuthorizationService authorizationService, IXLabFileService fileService)
        {
            try
            {
                _authorizationService = authorizationService;
                _authorizationService.AuthorizeAsync().Await();
                _fileService = fileService;
                _fileService.SetAuthorization();
                IsBusy = false;

                LoadFileCommand = new AsyncCommand(async () => await LoadFileAsync(), () => SelectedFile != null && !IsBusy);
                SaveFileCommand = new AsyncCommand(async () => await SaveFileAsync(), () => SelectedFile != null && !IsBusy);
                DeleteFileCommand = new AsyncCommand(async () => await DeleteFileAsync(), () => SelectedFile != null && !IsBusy);
                SelectFileCommand = new AsyncCommand(SelectFile, () => SelectedFile != null && !IsBusy);
                Files = _fileService.GetXLabFilesAsync().Await();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadFileAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                await SelectedFile.GetXLabFileDataAsync();
                OnPropertyChanged(nameof(SelectedFile));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveFileAsync()
        {
            if (SelectedFile == null) return;
            IsBusy = true;

            try
            {
                await SelectedFile.SaveXLabFileDataAsync();
                await _fileService.SaveXLabFileAsync(SelectedFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteFileAsync()
        {
            if (SelectedFile == null) return;
            IsBusy = true;

            try
            {
                await _fileService.DeleteXLabFileAsync(SelectedFile);
                Files.Remove(SelectedFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SelectFile()
        {
            IsBusy = true;
            var dialog = new OpenFileDialog
            {
                Title = "Выберите файл",
                Filter = "Все файлы (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                var fileData = await File.ReadAllBytesAsync(dialog.FileName);

                if (SelectedFile == null)
                {
                    SelectedFile = new XLabFile();
                }

                SelectedFile.FileName = Path.GetFileName(dialog.FileName);
                SelectedFile.FileData = Convert.ToBase64String(fileData);
                OnPropertyChanged(nameof(SelectedFile));
            }
            IsBusy = false;
        }
    }
}
