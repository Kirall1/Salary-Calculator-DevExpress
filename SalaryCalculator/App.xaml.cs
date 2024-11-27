using DevExpress.Xpf.Core;
using Microsoft.Extensions.DependencyInjection;
using SalaryCalculator.Data;
using SalaryCalculator.ViewModels;
using SalaryCalculator.Views;
using SalaryCalculator.Views.Windows;
using System;
using System.Windows;
using SalaryCalculator.Data.Repositories;
using SalaryCalculator.Data.Repositories.Impl;
using System.Globalization;
using System.Windows.Markup;

namespace SalaryCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            CultureInfo culture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
            XmlLanguage.GetLanguage(culture.IetfLanguageTag)));

            CompatibilitySettings.UseLightweightThemes = true;
            ServiceCollection serviceCollenction = new ServiceCollection();

            serviceCollenction.ConfigureServices();

            ServiceProvider serviceProvider = serviceCollenction.BuildServiceProvider();
            DbInitializer.Initialize(serviceProvider.GetRequiredService<DatabaseContext>());
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

            services.AddSingleton<SalaryDetailViewModel>();
            services.AddSingleton<SalaryDetailView>();

            services.AddDbContext<DatabaseContext>();
            services.AddTransient<IRankCoefficientRepository, RankCoefficientRepository>();
            services.AddTransient<ISalaryDetailRepository, SalaryDetailRepository>();
            services.AddTransient<IAdditionToSalaryRepository, AdditionToSalaryRepository>();
        }
    }
}
