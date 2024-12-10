using System;
using System.IO;
using OpenCvSharp;
using ImageMagick;
using SkiaSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using ImageCompressionResearchApp.Models;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Compression.Zlib;
using System.Diagnostics;
using ImageCompressionResearchApp.Servicies;

namespace ImageCompressionResearchApp.Services
{
    public static class ImageCompressionService
    {
        public static ImageModel CompressWithOpenCv(ImageModel image, int compressionPercentage, string outputFolder)
        {
            string newFileName = GenerateFileName(image.FileName, "OpenCv", compressionPercentage);
            string outputPath = Path.Combine(outputFolder, newFileName);

            Mat mat = Cv2.ImRead(Path.Combine(MainViewModel.OriginalImagesFolder, image.FileName));

            Tuple<long, float, double> perfomanceMetrcis;
            switch (image.Extension)
            {
                case ".png":
                    perfomanceMetrcis = MeasurePerformance(() => Cv2.ImWrite(outputPath, mat, new ImageEncodingParam(ImwriteFlags.PngCompression, compressionPercentage / 10)));
                    break;
                case ".jpg":
                case ".jpeg":
                    perfomanceMetrcis = MeasurePerformance(() => Cv2.ImWrite(outputPath, mat, new ImageEncodingParam(ImwriteFlags.JpegQuality, 100 - compressionPercentage)));
                    break;
                case ".webp":
                    perfomanceMetrcis = MeasurePerformance(() => Cv2.ImWrite(outputPath, mat, new ImageEncodingParam(ImwriteFlags.WebPQuality, 100 - compressionPercentage)));
                    break;
                default:
                    LogService.WriteLog(EventLogEntryType.Error, "IC", "AP", "00", "ImageCompressionService", "Unsupported image format");
                    throw new Exception("Неподдерживаемый формат изображения");
            }

            var newSize = new FileInfo(outputPath).Length / 1024.0;
            return new ImageModel
            {
                FileName = newFileName,
                Extension = image.Extension,
                Width = mat.Width,
                Height = mat.Height,
                FileSizeKb = newSize,
                Psnr = ComputePSNR(Path.Combine(MainViewModel.OriginalImagesFolder, image.FileName), outputPath),
                TimeOfWorkMs  = perfomanceMetrcis.Item1,
                CpuUsage = perfomanceMetrcis.Item2,
                MemoryUsageKb = perfomanceMetrcis.Item3
            };
        }

        public static ImageModel CompressWithMagick(ImageModel image, int compressionPercentage, string outputFolder)
        {
            if (image.Extension != ".jpg" && image.Extension != ".jpeg" &&
                image.Extension != ".png" && image.Extension != ".miff")
            {
                LogService.WriteLog(EventLogEntryType.Error, "IC", "AP", "00", "ImageCompressionService", "Unsupported image format");
                throw new Exception("Неподдерживаемый формат изображения");
            }
            string newFileName = GenerateFileName(image.FileName, "Magick", compressionPercentage);
            string outputPath = Path.Combine(outputFolder, newFileName);

            Tuple<long, float, double> perfomanceMetrcis;
            using (var magickImage = new MagickImage(Path.Combine(MainViewModel.OriginalImagesFolder, image.FileName)))
            {
                perfomanceMetrcis = MeasurePerformance(() =>
                {
                    magickImage.Quality = Convert.ToUInt32(100 - compressionPercentage);
                    magickImage.Write(outputPath);
                });
            }

            using (var magickImage = new MagickImage(outputPath))
            {
                return new ImageModel
                {
                    FileName = newFileName,
                    Extension = image.Extension,
                    Width = Convert.ToInt32(magickImage.Width),
                    Height = Convert.ToInt32(magickImage.Height),
                    FileSizeKb = new FileInfo(outputPath).Length / 1024.0,
                    Psnr = ComputePSNR(Path.Combine(MainViewModel.OriginalImagesFolder, image.FileName), outputPath),
                    TimeOfWorkMs = perfomanceMetrcis.Item1,
                    CpuUsage = perfomanceMetrcis.Item2,
                    MemoryUsageKb = perfomanceMetrcis.Item3
                };
            }
        }

        public static ImageModel CompressWithSkiaSharp(ImageModel image, int compressionPercentage, string outputFolder)
        {
            string newFileName = GenerateFileName(image.FileName, "SkiaSharp", compressionPercentage);
            string outputPath = Path.Combine(outputFolder, newFileName);
            Tuple<long, float, double> perfomanceMetrcis;

            using (var input = File.OpenRead(Path.Combine(MainViewModel.OriginalImagesFolder, image.FileName)))
            using (var skBitmap = SKBitmap.Decode(input))
            using (var output = File.OpenWrite(outputPath))
            {
                SKImage skImage = SKImage.FromBitmap(skBitmap);
                switch (image.Extension)
                {
                    case ".png":
                        perfomanceMetrcis = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Png, 100 - compressionPercentage).SaveTo(output));
                        break;
                    case ".jpg":
                    case ".jpeg":
                        perfomanceMetrcis = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Jpeg, 100 - compressionPercentage).SaveTo(output));
                        break;
                    case ".webp":
                        perfomanceMetrcis = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Webp, 100 - compressionPercentage).SaveTo(output));
                        break;
                    case ".bmp":
                        perfomanceMetrcis = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Bmp, 100 - compressionPercentage).SaveTo(output));
                        break;
                    case ".gif":
                        perfomanceMetrcis = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Gif, 100 - compressionPercentage).SaveTo(output));
                        break;
                    case ".ico":
                        perfomanceMetrcis = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Ico, 100 - compressionPercentage).SaveTo(output));
                        break;
                    case ".wbmp":
                        perfomanceMetrcis = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Wbmp, 100 - compressionPercentage).SaveTo(output));
                        break;
                    case ".pkm":
                        perfomanceMetrcis = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Pkm, 100 - compressionPercentage).SaveTo(output));
                        break;
                    case ".ktx":
                        perfomanceMetrcis  = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Ktx, 100 - compressionPercentage).SaveTo(output));
                        break;
                    case ".astc":
                        perfomanceMetrcis = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Astc, 100 - compressionPercentage).SaveTo(output));
                        break;
                    case ".dng":
                        perfomanceMetrcis = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Dng, 100 - compressionPercentage).SaveTo(output));
                        break;
                    case ".heif":
                        perfomanceMetrcis  = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Heif, 100 - compressionPercentage).SaveTo(output));
                        break;
                    case ".avif":
                        perfomanceMetrcis = MeasurePerformance(() => skImage.Encode(SKEncodedImageFormat.Avif, 100 - compressionPercentage).SaveTo(output));
                        break;
                    default:
                        LogService.WriteLog(EventLogEntryType.Error, "IC", "AP", "00", "ImageCompressionService", "Unsupported image format");
                        throw new Exception("Неподдерживаемый формат изображения");
                }
            }

            using (var skBitmap = SKBitmap.Decode(outputPath))
            {
                return new ImageModel
                {
                    FileName = newFileName,
                    Extension = image.Extension,
                    Width = skBitmap.Width,
                    Height = skBitmap.Height,
                    FileSizeKb = new FileInfo(outputPath).Length / 1024.0,
                    Psnr = ComputePSNR(Path.Combine(MainViewModel.OriginalImagesFolder, image.FileName), outputPath),
                    TimeOfWorkMs = perfomanceMetrcis.Item1,
                    CpuUsage = perfomanceMetrcis.Item2,
                    MemoryUsageKb = perfomanceMetrcis.Item3
                };
            }
        }

        public static ImageModel CompressWithImageSharp(ImageModel image, int compressionPercentage, string outputFolder)
        {
            string newFileName = GenerateFileName(image.FileName, "ImageSharp", compressionPercentage);
            string outputPath = Path.Combine(outputFolder, newFileName);
            Tuple<long, float, double> perfomanceMetrcis;

            using (var img = Image.Load(Path.Combine(MainViewModel.OriginalImagesFolder, image.FileName)))
            {

                img.Metadata.ExifProfile = null;
                switch (image.Extension)
                {
                    case ".png":
                        if(compressionPercentage == 100) compressionPercentage = 99;
                        perfomanceMetrcis = MeasurePerformance(() => img.SaveAsPng(outputPath, new PngEncoder { CompressionLevel = (PngCompressionLevel)(compressionPercentage / 10) }));
                        break;
                    case ".jpg":
                    case ".jpeg":
                        if (compressionPercentage == 100) compressionPercentage = 99;
                        perfomanceMetrcis = MeasurePerformance(() => img.SaveAsJpeg(outputPath, new JpegEncoder { Quality = 100 - compressionPercentage }));
                        break;
                    case ".webp":
                        if (compressionPercentage == 100) compressionPercentage = 99;
                        perfomanceMetrcis = MeasurePerformance(() => img.SaveAsWebp(outputPath, new WebpEncoder { Quality = 100 - compressionPercentage }));
                        break;
                    case ".tiff":
                        perfomanceMetrcis = MeasurePerformance(() => img.SaveAsTiff(outputPath, new TiffEncoder { CompressionLevel = (DeflateCompressionLevel)(compressionPercentage / 10) }));
                        break;
                    default:
                        LogService.WriteLog(EventLogEntryType.Error, "IC", "AP", "00", "ImageCompressionService", "Unsupported image format");
                        throw new Exception("Неподдерживаемый формат изображения");
                }
            }

            using (var img = Image.Load(outputPath))
            {
                return new ImageModel
                {
                    FileName = newFileName,
                    Extension = image.Extension,
                    Width = img.Width,
                    Height = img.Height,
                    FileSizeKb = new FileInfo(outputPath).Length / 1024.0,
                    Psnr = ComputePSNR(Path.Combine(MainViewModel.OriginalImagesFolder, image.FileName), outputPath),
                    TimeOfWorkMs = perfomanceMetrcis.Item1,
                    CpuUsage = perfomanceMetrcis.Item2,
                    MemoryUsageKb = perfomanceMetrcis.Item3
                };
            }
        }

        private static string GenerateFileName(string originalPath, string method, int compressionPercentage)
        {
            string originalFileName = Path.GetFileNameWithoutExtension(originalPath);
            string extension = Path.GetExtension(originalPath);
            return $"{originalFileName}_{method}_{compressionPercentage}{extension}";
        }

        public static double ComputePSNR(string originalPath, string compressedPath)
        {
            using var original = Cv2.ImRead(originalPath, ImreadModes.Color);
            using var compressed = Cv2.ImRead(compressedPath, ImreadModes.Color);

            return Cv2.PSNR(original, compressed);
        }

        public static Tuple<long, float, double> MeasurePerformance(Action action)
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            System.Threading.Thread.Sleep(500);


            var process = Process.GetCurrentProcess();
            long memoryBefore = GC.GetTotalMemory(true);

            var stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();

            long memoryAfter = GC.GetTotalMemory(false);
            float cpuUsage = cpuCounter.NextValue();

            return Tuple.Create(stopwatch.ElapsedMilliseconds, cpuUsage, (memoryAfter - memoryBefore) / 1024.0);
        }
    }
}
