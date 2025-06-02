// In a new file: Models/AppSettings.cs
// Or you can create a new folder, e.g., "Configuration" or "Settings" for these classes.
// For now, Models is fine.
using System.ComponentModel;

namespace AccessibleCryptoViewer.Models // Ensure this namespace matches your project
{
    public enum AppTheme
    {
        Light,
        Dark
        // We can add HighContrast later or handle it by listening to system settings
    }

    public enum CryptoDescriptionStyle
    {
        Verbose,
        Concise,
        Numerical
    }

    // Enum for the order of information might be more complex if it dictates actual layout.
    // For now, let's assume it's a choice that influences a predefined layout or data order.
    public enum CryptoInfoOrderPreset
    {
        Default, // e.g., Price, Change, Volume
        VolumeFirst,
        MarketCapFirst
        // Add more presets as defined
    }

    public class AppSettings : INotifyPropertyChanged
    {
        private AppTheme _selectedTheme = AppTheme.Light; // Default theme
        private int _refreshRateSeconds = 60; // Default refresh rate (e.g., 60 seconds)
        private bool _playUpdateBeep = true; // Default to playing beep
        private int _reservedApiCallCount = 2; // Default reserved calls
        private CryptoDescriptionStyle _selectedDescriptionStyle = CryptoDescriptionStyle.Verbose;
        private CryptoInfoOrderPreset _selectedCryptoInfoOrder = CryptoInfoOrderPreset.Default;
        private int _fontSizeAdjustment = 0; // e.g., -2, -1, 0, +1, +2 representing adjustments

        public AppTheme SelectedTheme
        {
            get => _selectedTheme;
            set { if (_selectedTheme != value) { _selectedTheme = value; OnPropertyChanged(nameof(SelectedTheme)); } }
        }

        public int RefreshRateSeconds
        {
            get => _refreshRateSeconds;
            set { if (_refreshRateSeconds != value) { _refreshRateSeconds = value; OnPropertyChanged(nameof(RefreshRateSeconds)); } }
        }

        public bool PlayUpdateBeep
        {
            get => _playUpdateBeep;
            set { if (_playUpdateBeep != value) { _playUpdateBeep = value; OnPropertyChanged(nameof(PlayUpdateBeep)); } }
        }

        public int ReservedApiCallCount
        {
            get => _reservedApiCallCount;
            set
            {
                // Add validation if necessary, e.g., ensure it's within 0-10
                if (value < 0) value = 0;
                if (value > 10) value = 10; // Example upper limit
                if (_reservedApiCallCount != value) { _reservedApiCallCount = value; OnPropertyChanged(nameof(ReservedApiCallCount)); }
            }
        }

        public CryptoDescriptionStyle SelectedDescriptionStyle
        {
            get => _selectedDescriptionStyle;
            set { if (_selectedDescriptionStyle != value) { _selectedDescriptionStyle = value; OnPropertyChanged(nameof(SelectedDescriptionStyle)); } }
        }

        public CryptoInfoOrderPreset SelectedCryptoInfoOrder
        {
            get => _selectedCryptoInfoOrder;
            set { if (_selectedCryptoInfoOrder != value) { _selectedCryptoInfoOrder = value; OnPropertyChanged(nameof(SelectedCryptoInfoOrder)); } }
        }
        
        public int FontSizeAdjustment
        {
            get => _fontSizeAdjustment;
            set
            {
                // Example: allow adjustment from -5 to +10
                if (value < -5) value = -5;
                if (value > 10) value = 10;
                if (_fontSizeAdjustment != value) { _fontSizeAdjustment = value; OnPropertyChanged(nameof(FontSizeAdjustment)); }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}