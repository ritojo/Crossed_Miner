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
        public ObservableList<String>   Servers { get; set; }

        public Setup()
        {
            Servers = new ObservableList<string>();
            Servers.Add("asia1.ethermine.org");
            Servers.Add("eu1.ethermine.org");
            Servers.Add("us1.ethermine.org");
            Servers.Add("us2.ethermine.org");

            InitializeComponent();

            if (Settings.Server != null)
            {
                serverComboBox.SelectedIndex = Servers.IndexOf(Settings.Server);
            }

            if (Settings.Wallet != null)
            {
                walletTextBox.Text = Settings.Wallet;
            }

            if (Settings.Worker != null)
            {
                workerTextBox.Text = Settings.Worker;
            }

            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (serverComboBox.SelectedItem != null)
            {
                Settings.Server = serverComboBox.SelectedItem.ToString();
            }

            if (walletTextBox.Text != null)
            {
                Settings.Wallet = walletTextBox.Text;
            }

            if (workerTextBox.Text != null)
            {
                Settings.Worker = workerTextBox.Text;
            }

            Settings.SaveSettings();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
