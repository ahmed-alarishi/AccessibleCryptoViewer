// In Managers/ThemeManager.cs
using System;
using System.Linq;
using System.Windows;
using AccessibleCryptoViewer.Models; // To access AppTheme enum

namespace AccessibleCryptoViewer.Managers // Ensure this namespace matches your project structure
{
    public static class ThemeManager
    {
        public static void ApplyTheme(AppTheme theme)
        {
            // Get the application's merged dictionaries
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries == null) return;

            // Remove any existing theme dictionaries.
            // This is a simple way; more robust might involve tagging dictionaries or knowing their exact position.
            var existingThemeDictionaries = mergedDictionaries
                .Where(rd => rd.Source != null && rd.Source.OriginalString.StartsWith("Themes/", StringComparison.OrdinalIgnoreCase))
                .ToList(); // ToList() is important to avoid modifying the collection while iterating

            foreach (var rd in existingThemeDictionaries)
            {
                mergedDictionaries.Remove(rd);
            }

            // Determine the URI of the new theme file
            string themeSourceUri;
            switch (theme)
            {
                case AppTheme.Dark:
                    themeSourceUri = "Themes/DarkTheme.xaml";
                    break;
                case AppTheme.Light:
                default:
                    themeSourceUri = "Themes/LightTheme.xaml";
                    break;
            }

            try
            {
                // Create and add the new theme dictionary
                var newThemeDictionary = new ResourceDictionary
                {
                    Source = new Uri(themeSourceUri, UriKind.RelativeOrAbsolute)
                };
                mergedDictionaries.Add(newThemeDictionary);
            }
            catch (Exception ex)
            {
                // Log this error (we'll add proper logging later)
                Console.WriteLine($"Error applying theme '{themeSourceUri}': {ex.Message}");
                // Optionally, try to reapply a default theme if the new one fails
                // For now, we just log.
            }
        }
    }
}