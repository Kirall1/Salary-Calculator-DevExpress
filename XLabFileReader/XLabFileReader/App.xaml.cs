using DevExpress.Xpf.Core;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using XLabFileReader.Services.Interfaces;
using XLabFileReader.Services;
using XLabFileReader.ViewModels;

namespace XLabFileReader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            CompatibilitySettings.UseLightweightThemes = true;

            ServiceCollection serviceCollenction = new ServiceCollection();

            serviceCollenction.ConfigureServices();
            ServiceProvider serviceProvider = serviceCollenction.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }


    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IXLabFileService, XLabFileService>();
        }
    }
}
