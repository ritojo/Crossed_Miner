using System.Windows;

namespace Crossed_Miner
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();

            // Load Settings
            //Settings.LoadSettings();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetupButton_Click(object sender, RoutedEventArgs e)
        {
            Setup setupWindow = new Setup();
            setupWindow.ShowDialog();
        }
    }
}
