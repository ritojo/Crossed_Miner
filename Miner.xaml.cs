using System;
using System.Collections.Generic;
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
        private static System.Timers.Timer timer = new System.Timers.Timer(200);
        private DateTime mineStart = new DateTime();

        public Miner()
        {
            InitializeComponent();
            ReloadUI();
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

            string args = "-a ethash -o stratum+tcp://" + Settings.Server + ":4444 -u " + Settings.Wallet + " -p x -w " + Settings.Worker;

            if(state == MiningState.MINING)
            {
                myConsole.WriteOutput("Stopping Mining", Color.FromRgb(255, 255, 255));
                StopTimer();
                myConsole.StopProcess();
                state = MiningState.STOPPED;
            }
            else
            {
                myConsole.WriteOutput("Starting Mining", Color.FromRgb(255, 255, 255));
                myConsole.StartProcess(exeName, args);
                state = MiningState.MINING;
                StartTimer();
            }
            ReloadUI();
        }

        private void StartTimer()
        {
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void StopTimer()
        {
            timer.AutoReset = false;
            timer.Enabled = false;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at " +  e.SignalTime);
        }
    }
}
