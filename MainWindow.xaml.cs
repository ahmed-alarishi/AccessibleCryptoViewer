// MainWindow.xaml.cs
using System;
using System.Collections.Generic; // For List<string>
using System.Collections.ObjectModel; // For ObservableCollection
using System.Linq; // For ComboBox item iteration if needed
using System.Threading.Tasks; // For Task
using System.Windows;
using System.Windows.Controls;
using AccessibleCryptoViewer.Models;
using AccessibleCryptoViewer.Managers;
using AccessibleCryptoViewer.Services; // For CoinGeckoService

namespace AccessibleCryptoViewer // ENSURE THIS NAMESPACE MATCHES YOUR PROJECT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CoinGeckoService _coinGeckoService;
        public ObservableCollection<CryptoCurrency> WatchlistCryptos { get; set; }

        public MainWindow()
        {
            InitializeComponent(); // This call links XAML to this C# code

            _coinGeckoService = new CoinGeckoService(); 
            WatchlistCryptos = new ObservableCollection<CryptoCurrency>();
            WatchlistCryptoListBox.ItemsSource = WatchlistCryptos;

            LoadAndSetInitialSettings();
            // The Window_Loaded event (defined in XAML) will trigger the initial data load.
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Log that the method is called
            RuntimeLogger.Log("Debug: Window_Loaded event fired");
            await LoadWatchlistDataAsync();
        }

        private async void RefreshListButton_Click(object sender, RoutedEventArgs e)
        {
            RuntimeLogger.Log("Debug: RefreshListButton_Click event fired");
            await LoadWatchlistDataAsync();
        }

        private async Task LoadWatchlistDataAsync()
        {
            RuntimeLogger.Log("Debug: LoadWatchlistDataAsync started.");
            DataStatusTextBlock.Text = "Status: Loading data...";
            LastUpdatedTextBlock.Text = "Last updated: Fetching...";

            var coinIdsToFetch = new List<string> { "bitcoin", "ethereum", "solana", "cardano", "polkadot", "dogecoin" };

            try
            {
                List<CryptoCurrency> fetchedCryptos = await _coinGeckoService.GetMarketDataAsync(coinIdsToFetch);

                Application.Current.Dispatcher.Invoke(() => // Ensure UI updates are on the UI thread
                {
                    WatchlistCryptos.Clear(); 
                    if (fetchedCryptos != null && fetchedCryptos.Any())
                    {
                        foreach (var crypto in fetchedCryptos)
                        {
                            WatchlistCryptos.Add(crypto);
                        }
                        DataStatusTextBlock.Text = "Status: OK";
                        LastUpdatedTextBlock.Text = $"Last updated: {DateTime.Now:g}"; 
                        RuntimeLogger.Log($"Debug: Watchlist loaded with {fetchedCryptos.Count} items.");
                    }
                    else
                    {
                        DataStatusTextBlock.Text = "Status: No data received or API error.";
                        LastUpdatedTextBlock.Text = $"Last updated: {DateTime.Now:g} (no data)";
                        RuntimeLogger.Log("Debug: LoadWatchlistDataAsync - No data received or API error from service.");
                    }
                });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DataStatusTextBlock.Text = "Status: Error loading data.";
                    LastUpdatedTextBlock.Text = $"Last updated: {DateTime.Now:g} (error)";
                });
                RuntimeLogger.Log($"Error in LoadWatchlistDataAsync: {ex.Message}");
            }
        }

        // --- Settings Event Handlers ---
        private void LoadAndSetInitialSettings()
        {
            RuntimeLogger.Log("Debug: LoadAndSetInitialSettings called.");
            if (App.AppSettingsService == null || App.AppSettingsService.CurrentSettings == null)
            {
                RuntimeLogger.Log("Error: Settings service not available in LoadAndSetInitialSettings. Using default UI values.");
                // Attempt to gracefully handle missing settings service for UI population
                try {
                    ThemeComboBox.ItemsSource = Enum.GetValues(typeof(AppTheme));
                    ThemeComboBox.SelectedIndex = 0;
                    DescriptionStyleComboBox.ItemsSource = Enum.GetValues(typeof(CryptoDescriptionStyle));
                    DescriptionStyleComboBox.SelectedIndex = 0;
                } catch (Exception ex) {
                    RuntimeLogger.Log($"Error initializing ComboBoxes with defaults: {ex.Message}");
                }
                return;
            }

            var settings = App.AppSettingsService.CurrentSettings;

            ThemeComboBox.ItemsSource = Enum.GetValues(typeof(AppTheme));
            ThemeComboBox.SelectedItem = settings.SelectedTheme;

            foreach (ComboBoxItem item in RefreshRateComboBox.Items)
            {
                if (item.Tag?.ToString() == settings.RefreshRateSeconds.ToString())
                {
                    RefreshRateComboBox.SelectedItem = item;
                    break;
                }
            }
            if (RefreshRateComboBox.SelectedItem == null && RefreshRateComboBox.Items.Count > 0) RefreshRateComboBox.SelectedIndex = 0;

            DescriptionStyleComboBox.ItemsSource = Enum.GetValues(typeof(CryptoDescriptionStyle));
            DescriptionStyleComboBox.SelectedItem = settings.SelectedDescriptionStyle;
            if (DescriptionStyleComboBox.SelectedItem == null && DescriptionStyleComboBox.Items.Count > 0) DescriptionStyleComboBox.SelectedIndex = 0;

            ReservedApiCallCountTextBox.Text = settings.ReservedApiCallCount.ToString();
            FontSizeAdjustmentTextBox.Text = settings.FontSizeAdjustment.ToString();
            BeepOnUpdateCheckBox.IsChecked = settings.PlayUpdateBeep;
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeComboBox.SelectedItem is AppTheme selectedTheme && App.AppSettingsService?.CurrentSettings != null)
            {
                if (App.AppSettingsService.CurrentSettings.SelectedTheme != selectedTheme)
                {
                    App.AppSettingsService.CurrentSettings.SelectedTheme = selectedTheme;
                    App.AppSettingsService.SaveSettings();
                    ThemeManager.ApplyTheme(selectedTheme);
                    RuntimeLogger.Log($"Theme changed to: {selectedTheme}");
                }
            }
        }

        private void RefreshRateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RefreshRateComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null && App.AppSettingsService?.CurrentSettings != null)
            {
                if (int.TryParse(selectedItem.Tag.ToString(), out int refreshSeconds))
                {
                    if (App.AppSettingsService.CurrentSettings.RefreshRateSeconds != refreshSeconds)
                    {
                        App.AppSettingsService.CurrentSettings.RefreshRateSeconds = refreshSeconds;
                        App.AppSettingsService.SaveSettings();
                        RuntimeLogger.Log($"Refresh rate changed to: {refreshSeconds}s");
                    }
                }
            }
        }

        private void DescriptionStyleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DescriptionStyleComboBox.SelectedItem is CryptoDescriptionStyle selectedStyle && App.AppSettingsService?.CurrentSettings != null)
            {
                if (App.AppSettingsService.CurrentSettings.SelectedDescriptionStyle != selectedStyle)
                {
                    App.AppSettingsService.CurrentSettings.SelectedDescriptionStyle = selectedStyle;
                    App.AppSettingsService.SaveSettings();
                    RuntimeLogger.Log($"Description style changed to: {selectedStyle}");
                }
            }
        }

        private void ReservedApiCallCountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Basic update during typing, final validation on LostFocus
            if (int.TryParse(ReservedApiCallCountTextBox.Text, out int count) && App.AppSettingsService?.CurrentSettings != null)
            {
                // Apply validation constraints from AppSettings if it had them (0-10)
                int validatedCount = count;
                if (validatedCount < 0) validatedCount = 0;
                if (validatedCount > 10) validatedCount = 10; 

                if (App.AppSettingsService.CurrentSettings.ReservedApiCallCount != validatedCount)
                {
                    App.AppSettingsService.CurrentSettings.ReservedApiCallCount = validatedCount;
                    // Save happens on LostFocus
                }
            }
        }

        private void FontSizeAdjustmentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Basic update during typing, final validation and application on LostFocus
            if (int.TryParse(FontSizeAdjustmentTextBox.Text, out int adjustment) && App.AppSettingsService?.CurrentSettings != null)
            {
                int validatedAdjustment = adjustment;
                if (validatedAdjustment < -5) validatedAdjustment = -5;
                if (validatedAdjustment > 10) validatedAdjustment = 10;

                if (App.AppSettingsService.CurrentSettings.FontSizeAdjustment != validatedAdjustment)
                {
                    App.AppSettingsService.CurrentSettings.FontSizeAdjustment = validatedAdjustment;
                    FontManager.ApplyFontSizeAdjustment(validatedAdjustment); // Apply live
                    // Save happens on LostFocus
                }
            }
        }
        
        // This is the event handler your XAML is looking for
        private void TextBox_Lost_Focus_NumericValidation(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && App.AppSettingsService?.CurrentSettings != null)
            {
                bool changed = false;
                if (textBox == ReservedApiCallCountTextBox)
                {
                    if (int.TryParse(textBox.Text, out int count))
                    {
                        int originalValue = App.AppSettingsService.CurrentSettings.ReservedApiCallCount;
                        int validatedCount = count;
                        if (validatedCount < 0) validatedCount = 0;
                        if (validatedCount > 10) validatedCount = 10;
                        textBox.Text = validatedCount.ToString(); // Update TextBox with validated value
                        if (originalValue != validatedCount)
                        {
                             App.AppSettingsService.CurrentSettings.ReservedApiCallCount = validatedCount;
                             changed = true;
                        }
                    }
                    else textBox.Text = App.AppSettingsService.CurrentSettings.ReservedApiCallCount.ToString(); // Revert if invalid
                }
                else if (textBox == FontSizeAdjustmentTextBox)
                {
                    if (int.TryParse(textBox.Text, out int adjustment))
                    {
                        int originalValue = App.AppSettingsService.CurrentSettings.FontSizeAdjustment;
                        int validatedAdjustment = adjustment;
                        if (validatedAdjustment < -5) validatedAdjustment = -5;
                        if (validatedAdjustment > 10) validatedAdjustment = 10;
                        textBox.Text = validatedAdjustment.ToString(); // Update TextBox with validated value
                        if (originalValue != validatedAdjustment)
                        {
                            App.AppSettingsService.CurrentSettings.FontSizeAdjustment = validatedAdjustment;
                            FontManager.ApplyFontSizeAdjustment(validatedAdjustment); 
                            changed = true;
                        }
                    }
                    else textBox.Text = App.AppSettingsService.CurrentSettings.FontSizeAdjustment.ToString(); // Revert if invalid
                }

                if(changed)
                {
                    App.AppSettingsService.SaveSettings();
                    RuntimeLogger.Log($"Settings saved due to TextBox LostFocus. ReservedCalls: {App.AppSettingsService.CurrentSettings.ReservedApiCallCount}, FontSizeAdj: {App.AppSettingsService.CurrentSettings.FontSizeAdjustment}");
                }
            }
        }

        private void BeepOnUpdateCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (BeepOnUpdateCheckBox.IsChecked.HasValue && App.AppSettingsService?.CurrentSettings != null)
            {
                if (App.AppSettingsService.CurrentSettings.PlayUpdateBeep != BeepOnUpdateCheckBox.IsChecked.Value)
                {
                    App.AppSettingsService.CurrentSettings.PlayUpdateBeep = BeepOnUpdateCheckBox.IsChecked.Value;
                    App.AppSettingsService.SaveSettings();
                    RuntimeLogger.Log($"Beep on update changed to: {App.AppSettingsService.CurrentSettings.PlayUpdateBeep}");
                }
            }
        }
    }
}