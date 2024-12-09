namespace ImageCompressionResearchApp.Models
{
    public class ImageModel : ViewModel
    {
        string _fileName;
        public string FileName
        {
            get => _fileName;
            set => Set(ref _fileName, value);
        }

        string _extension;
        public string Extension
            {
            get => _extension;
            set => Set(ref _extension, value);
        }

        int _width;
        public int Width
        {
            get => _width;
            set => Set(ref _width, value);
        }

        int _height;
        public int Height
            {
            get => _height;
            set => Set(ref _height, value);
        }

        double _fileSizeKb;
        public double FileSizeKb
        {
            get => _fileSizeKb;
            set => Set(ref _fileSizeKb, value);
        }

        double? _psnr;
        public double? Psnr
        {
            get => _psnr;
            set => Set(ref _psnr, value);
        }

        long? _timeOfWorkMs;
        public long? TimeOfWorkMs
        {
            get => _timeOfWorkMs;
            set => Set(ref _timeOfWorkMs, value);
        }

        float? _cpuUsage;
        public float? CpuUsage
        {
            get => _cpuUsage;
            set => Set(ref _cpuUsage, value);
        }

        double? _memoryUsageKb;
        public double? MemoryUsageKb
        {
            get => _memoryUsageKb;
            set => Set(ref _memoryUsageKb, value);
        }
    }
}
