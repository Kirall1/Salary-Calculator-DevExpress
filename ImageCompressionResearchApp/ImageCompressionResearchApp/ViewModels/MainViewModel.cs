using DevExpress.Mvvm;
using ImageCompressionResearchApp.Models;
using ImageCompressionResearchApp.Services;
using ImageCompressionResearchApp.Servicies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using System.Windows.Input;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

public class MainViewModel : ViewModel
{
    private const string CompressedImagesFile = "CompressedImagesData.json";

    private ImageModel _selectedImage;
    public ImageModel SelectedImage
    {
        get => _selectedImage;
        set
        {
            if (value == null) return;
            Set(ref _selectedImage, value);
        }
    }

    private int _compressionPercentage = 50;
    public int CompressionPercentage
    {
        get => _compressionPercentage;
        set => Set(ref _compressionPercentage, value);
    }

    public ObservableCollection<ImageModel> OriginalImages { get; }
    public ObservableCollection<ImageModel> CompressedImages { get; }

    public static string OriginalImagesFolder { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OriginalImages");
    public static string CompressedImagesFolder { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CompressedImages");

    public ICommand RefreshImagesCommand { get; }
    public ICommand SelectImageCommand { get; }
    public ICommand SelectOutputFolderCommand { get; }
    public ICommand CompressWithOpenCvCommand { get; }
    public ICommand CompressWithMagickCommand { get; }
    public ICommand CompressWithSkiaSharpCommand { get; }
    public ICommand CompressWithImageSharpCommand { get; }
    public ICommand OpenOriginalImageCommand { get; }
    public ICommand OpenCompressedImageCommand { get; }

    public MainViewModel()
    {
        OriginalImages = new ObservableCollection<ImageModel>();
        CompressedImages = new ObservableCollection<ImageModel>();

        RefreshImagesCommand = new DelegateCommand(RefreshImages);

        Directory.CreateDirectory(OriginalImagesFolder);
        Directory.CreateDirectory(CompressedImagesFolder);

        LoadCompressedImages();
        RefreshImages();

        SelectImageCommand = new DelegateCommand(SelectImage);

        CompressWithOpenCvCommand = new DelegateCommand(() => CompressImage(ImageCompressionService.CompressWithOpenCv));
        CompressWithMagickCommand = new DelegateCommand(() => CompressImage(ImageCompressionService.CompressWithMagick));
        CompressWithSkiaSharpCommand = new DelegateCommand(() => CompressImage(ImageCompressionService.CompressWithSkiaSharp));
        CompressWithImageSharpCommand = new DelegateCommand(() => CompressImage(ImageCompressionService.CompressWithImageSharp));
        OpenOriginalImageCommand = new DelegateCommand<ImageModel>(OpenOriginalImage);
        OpenCompressedImageCommand = new DelegateCommand<ImageModel>(OpenCompressedImage);
    }

    private void SelectImage()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.webp;*.bmp;*.ico;*.wbmp;*.pkm;*.ktx;*.astc;*.dng;*.heif;*.avif;*.miff",
            Title = "Выберите изображение"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string filePath = openFileDialog.FileName;
            FileInfo fileInfo = new FileInfo(filePath);

            Directory.CreateDirectory(OriginalImagesFolder);

            string destinationPath = Path.Combine(OriginalImagesFolder, Path.GetFileName(filePath));

            try
            {
                File.Copy(filePath, destinationPath, overwrite: true);

                using (System.Drawing.Image image = System.Drawing.Image.FromFile(destinationPath))
                {
                    SelectedImage = new ImageModel
                    {
                        FileName = Path.GetFileName(destinationPath),
                        Extension = fileInfo.Extension,
                        Height = image.Height,
                        Width = image.Width,
                        FileSizeKb = fileInfo.Length / 1024.0
                    };

                    OriginalImages.Add(SelectedImage);
                }
            }
            catch (Exception ex)
            {
                LogService.WriteLog(EventLogEntryType.Error, "MV", "AP", "00", "MainViewModel", ex.Message);
                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }


    private void OpenOriginalImage(ImageModel imageModel)
    {
        OpenImage(imageModel, OriginalImagesFolder);
    }

    private void OpenCompressedImage(ImageModel imageModel)
    {
        OpenImage(imageModel, CompressedImagesFolder);
    }


    private void OpenImage(ImageModel imageModel, string folder)
    {
        if (imageModel == null || string.IsNullOrEmpty(imageModel.FileName))
            return;

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Path.Combine(folder, imageModel.FileName),
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            LogService.WriteLog(EventLogEntryType.Error, "MV", "AP", "01", "MainViewModel", ex.Message);
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RefreshImages()
    {
        LoadImagesFromFolder(OriginalImagesFolder, OriginalImages);
    }

    private void LoadImagesFromFolder(string folderPath, ObservableCollection<ImageModel> targetCollection)
    {
        targetCollection.Clear();

        if (!Directory.Exists(folderPath))
            return;

        var supportedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".ico" };
        var files = Directory.GetFiles(folderPath)
                             .Where(file => supportedExtensions.Contains(Path.GetExtension(file).ToLower()));

        foreach (var file in files)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(file);

                using (var stream = File.OpenRead(file))
                using (var skBitmap = SkiaSharp.SKBitmap.Decode(stream))
                {
                    if (skBitmap != null)
                    {
                        targetCollection.Add(new ImageModel
                        {
                            FileName = fileInfo.Name,
                            Extension = fileInfo.Extension,
                            Height = skBitmap.Height,
                            Width = skBitmap.Width,
                            FileSizeKb = fileInfo.Length / 1024.0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteLog(EventLogEntryType.Warning, "MV", "AP", "03", "MainViewModel", ex.Message);
            }
        }
    }


    private void SaveCompressedImages()
    {
        try
        {
            var json = JsonSerializer.Serialize(CompressedImages, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(CompressedImagesFile, json);
        }
        catch (Exception ex)
        {
            LogService.WriteLog(EventLogEntryType.Error, "MV", "AP", "04", "MainViewModel", ex.Message);
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadCompressedImages()
    {
        if (!File.Exists(CompressedImagesFile))
            return;

        try
        {
            var json = File.ReadAllText(CompressedImagesFile);
            var loadedImages = JsonSerializer.Deserialize<List<ImageModel>>(json);

            if (loadedImages != null)
            {
                foreach (var image in loadedImages)
                {
                    CompressedImages.Add(image);
                }
            }
        }
        catch (Exception ex)
        {
            LogService.WriteLog(EventLogEntryType.Error, "MV", "AP", "05", "MainViewModel", ex.Message);
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CompressImage(Func<ImageModel, int, string, ImageModel> compressionMethod)
    {
        if (SelectedImage == null)
        {
            System.Windows.MessageBox.Show("Пожалуйста, выберите изображение");
            return;
        }

        try
        {
            var compressedImage = compressionMethod(SelectedImage, CompressionPercentage, CompressedImagesFolder);
            CompressedImages.Add(compressedImage);

            SaveCompressedImages();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
