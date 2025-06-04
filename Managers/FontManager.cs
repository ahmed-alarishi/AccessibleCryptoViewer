// In Managers/FontManager.cs
using System;
using System.Windows;

namespace AccessibleCryptoViewer.Managers // Ensure this namespace is correct
{
    public static class FontManager
    {
        // Define a default base font size. This could also come from AppSettings if needed.
        private const double DefaultApplicationFontSize = 12.0; // WPF default is often around 12-14 for system font

        public static void ApplyFontSizeAdjustment(int adjustment)
        {
            try
            {
                double newBaseFontSize = DefaultApplicationFontSize + adjustment;
                if (newBaseFontSize < 8) newBaseFontSize = 8; // Minimum font size
                if (newBaseFontSize > 30) newBaseFontSize = 30; // Maximum font size

                // Option 1: Change a resource (requires elements to use DynamicResource for FontSize)
                // This is more robust for widespread changes.
                if (Application.Current.Resources.Contains("AppBaseFontSize"))
                {
                    Application.Current.Resources["AppBaseFontSize"] = newBaseFontSize;
                }
                else
                {
                    Application.Current.Resources.Add("AppBaseFontSize", newBaseFontSize);
                }

                // Option 2: Directly change MainWindow's FontSize (simpler for immediate effect on one window)
                // If your MainWindow and its children inherit FontSize, this can work.
                // However, explicit FontSizes on child elements will override this.
                // if (Application.Current.MainWindow != null)
                // {
                //     Application.Current.MainWindow.FontSize = newBaseFontSize;
                // }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying font size adjustment: {ex.Message}");
            }
        }
    }
}