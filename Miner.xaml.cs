using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Crossed_Miner
{
    // Mining State
    public enum MiningState
    {
        STOPPED,
        MINING
    }

    /// <summary>
    /// Interaction logic for Miner.xaml
    /// </summary>
    public partial class Miner : Page
    {
        // Instance Variables
        private MiningState state = MiningState.STOPPED;
        private const string exeName = "t-rex.exe";
        private ProcessStartInfo processStartInfo;
        private Process process;

        public Miner()
        {
            InitializeComponent();
            ReloadUI();
            SetupProcess();
        }

        public void ReloadUI()
        {
            if(state == MiningState.STOPPED)
            {
                StartMiningButton.Content = "Start Mining";
            }
            else
            {
                StartMiningButton.Content = "Stop Mining";
            }

        }

        private void SetupProcess()
        {
            processStartInfo = new ProcessStartInfo();
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.FileName = exeName;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        }

        private void StartMiningButton_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Server == null || Settings.Wallet == null || Settings.Worker == null)
            {
                Setup setupWindow = new Setup();
                setupWindow.ShowDialog();
            }

            if(!System.IO.File.Exists(exeName))
            {
                MessageBox.Show(Application.Current.MainWindow, "You must add t-rex.exe to this folder before mining.", "You Suck", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            processStartInfo.Arguments = "-a ethash --api-bind-telnet 0 --api-bind-http 127.0.0.1:4067 -o stratum+tcp://" + Settings.Server + ":4444 -u " + Settings.Wallet + " -p x -w " + Settings.Worker;

            if(state == MiningState.MINING)
            {
                myConsole.WriteOutput("Stopping Mining\n", Color.FromRgb(255, 255, 255));
                state = MiningState.STOPPED;
                process.Kill();
            }
            else
            {
                myConsole.WriteOutput("Starting Mining\n", Color.FromRgb(255, 255, 255));
                state = MiningState.MINING;
                process = Process.Start(processStartInfo);
            }
            ReloadUI();
        }
    }
}
