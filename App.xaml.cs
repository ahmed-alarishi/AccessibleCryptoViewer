// App.xaml.cs
using System.Windows;
using AccessibleCryptoViewer.Services; // For SettingsService
using AccessibleCryptoViewer.Managers; // For ThemeManager and FontManager
using AccessibleCryptoViewer.Models;   // For AppTheme enum

namespace AccessibleCryptoViewer // ENSURE THIS NAMESPACE MATCHES YOUR PROJECT
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

            // Apply the saved theme and font size on startup
            if (AppSettingsService.CurrentSettings != null) 
            {
                ThemeManager.ApplyTheme(AppSettingsService.CurrentSettings.SelectedTheme);
                FontManager.ApplyFontSizeAdjustment(AppSettingsService.CurrentSettings.FontSizeAdjustment);
            }
            else
            {
                // Fallback or error handling if settings couldn't be loaded
                ThemeManager.ApplyTheme(AppTheme.Light); // Default to Light
                FontManager.ApplyFontSizeAdjustment(0);  // Default adjustment
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Ensure settings are saved when the application exits
            AppSettingsService?.SaveSettings(); 
            base.OnExit(e);
        }
    }
}