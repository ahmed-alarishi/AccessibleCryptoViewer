// App.xaml.cs
using System.Windows;
using AccessibleCryptoViewer.Services; // Make sure this matches your namespace

namespace AccessibleCryptoViewer // Ensure this namespace matches your project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SettingsService AppSettingsService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppSettingsService = new SettingsService();

            // You can now access settings via App.AppSettingsService.CurrentSettings
            // For example, to apply a theme or set an initial window size based on settings.
            // We will handle applying the theme in MainWindow or a ThemeManager later.
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Ensure settings are saved when the application exits
            AppSettingsService?.SaveSettings(); 
            base.OnExit(e);
        }
    }
}