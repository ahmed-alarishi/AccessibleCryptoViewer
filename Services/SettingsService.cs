// In a new file: Services/SettingsService.cs
using AccessibleCryptoViewer.Models; // To access AppSettings
using System;
using System.IO;
using System.Text.Json; // For JSON serialization

namespace AccessibleCryptoViewer.Services // Ensure this namespace matches your project
{
    public class SettingsService
    {
        private readonly string _settingsFilePath;
        private const string SettingsFileName = "appsettings.json";

        public AppSettings CurrentSettings { get; private set; }

        public SettingsService()
        {
            // Define the path to the settings file
            // Using LocalApplicationData is a good place for user-specific configuration files
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appSpecificFolder = Path.Combine(appDataPath, "AccessibleCryptoViewer"); // Your app's folder
            
            // Ensure the directory exists
            Directory.CreateDirectory(appSpecificFolder); 
            
            _settingsFilePath = Path.Combine(appSpecificFolder, SettingsFileName);

            CurrentSettings = LoadSettings();
        }

        private AppSettings LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    string json = File.ReadAllText(_settingsFilePath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);
                    return settings ?? new AppSettings(); // Return new defaults if deserialization fails
                }
            }
            catch (Exception ex)
            {
                // Log the exception (we'll need a logging mechanism later)
                Console.WriteLine($"Error loading settings: {ex.Message}");
                // Fallback to default settings
            }
            return new AppSettings(); // Return default settings if file doesn't exist or error occurs
        }

        public void SaveSettings()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true }; // For pretty printing
                string json = JsonSerializer.Serialize(CurrentSettings, options);
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        // Example of how a setting might be updated and saved
        public void UpdateTheme(AppTheme newTheme)
        {
            if (CurrentSettings.SelectedTheme != newTheme)
            {
                CurrentSettings.SelectedTheme = newTheme;
                SaveSettings(); // Save whenever a setting changes
            }
        }

        // Add similar methods for other settings if you want to encapsulate saving,
        // or the UI/ViewModels can modify CurrentSettings properties directly and then call SaveSettings().
        // For properties that implement INotifyPropertyChanged on AppSettings, direct modification is fine,
        // but ensure SaveSettings() is called appropriately (e.g., on window closing, or after a batch of changes).
    }
}