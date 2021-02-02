using Gstc.Collections.Observable;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Crossed_Miner
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup : Window
    {
        private Settings settings;

        public ObservableList<String> Servers = new ObservableList<string>() { "asia1.ethermine.org", "eu1.ethermine.org", "us1.ethermine.org", "us2.ethermine.org" };

        public Setup( Settings settings)
        {
            this.settings = settings;

            Servers = new ObservableList<string>();

            InitializeComponent();

            if (settings.Server != null)
            {
                serverComboBox.SelectedIndex = Servers.IndexOf(settings.Server);
            }

            if (settings.Wallet != null)
            {
                walletTextBox.Text = settings.Wallet;
            }

            if (settings.Worker != null)
            {
                workerTextBox.Text = settings.Worker;
            }

            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (serverComboBox.SelectedItem != null)
            {
                settings.Server = serverComboBox.SelectedItem.ToString();
            }

            if (walletTextBox.Text != null)
            {
                settings.Wallet = walletTextBox.Text;
            }

            if (workerTextBox.Text != null)
            {
                settings.Worker = workerTextBox.Text;
            }

            settings.SaveSettings();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
