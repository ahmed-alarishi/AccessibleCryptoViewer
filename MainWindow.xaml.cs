using System.Windows;
using System.Windows.Controls; // Required for ComboBox, CheckBox etc. if you were to interact in code-behind

namespace AccessibleCryptoViewer// Ensure this namespace matches your project's name
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // You could set default selections for ComboBoxes here if needed, for example:
            // RefreshRateComboBox.SelectedIndex = 1; // Selects "30 seconds"
            // DescriptionStyleComboBox.SelectedIndex = 0; // Selects "Verbose"
        }

        // We will add event handlers and other logic here in later stages.
        // For example, what happens when a button is clicked or a setting is changed.
    }
}