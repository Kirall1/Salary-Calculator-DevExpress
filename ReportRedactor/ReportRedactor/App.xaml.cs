using DevExpress.Xpf.Core;
using System.Globalization;
using System.Windows;

namespace ReportRedactor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            CompatibilitySettings.UseLightweightThemes = true;
            CultureInfo culture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
