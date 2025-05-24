using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CryptoAccessible
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(Views.DashboardPage));
        }
    }
}
