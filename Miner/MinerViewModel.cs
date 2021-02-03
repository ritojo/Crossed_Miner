using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Crossed_Miner
{
    public class MinerViewModel : ObservableBase
    {
        private const string exeName = "t-rex.exe"; //TODO: move to settings/json/config, including relative file path (keep in same dir as this exe?)
        private ProcessStartInfo processStartInfo;
        //private Process process;
        private PersonalLog personalLog;

        public MinerViewModel()
        {
            processStartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = exeName,
                WindowStyle = ProcessWindowStyle.Hidden
            };
        }

        private bool isMining = false;
        public bool IsMining
        {
            get
            {
                return isMining;
            }
            set
            {
                if (isMining != value)
                {
                    isMining = value;
                    OnPropertyChanged("IsMining");
                }
            }
        }

        private ICommand toggleMiningCommand = null;
        public ICommand ToggleMiningCommand
        {
            get
            {
                if (toggleMiningCommand == null)
                {
                    toggleMiningCommand = new RelayCommand(param => ToggleMining());
                }

                return toggleMiningCommand;
            }
        }

        private void ToggleMining()
        {
            if (IsMining)
            {
                StartMining();
            }
            else
            {
                StopMining();
            }
        }

        private void StartMining()
        {
            if (!System.IO.File.Exists(exeName))
            {
                MessageBox.Show(Application.Current.MainWindow, "You must add t-rex.exe to this folder before mining.", "You Suck", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            CheckSettings();

            processStartInfo.Arguments = "-a ethash --api-bind-telnet 0 --api-bind-http 127.0.0.1:4067 -o stratum+tcp://" + Settings.Server + ":4444 -u " + Settings.Wallet + " -p x -w " + Settings.Worker;
            personalLog = new PersonalLog();
            //process = Process.Start(processStartInfo);
        }

        private void StopMining()
        {
            personalLog.StopLogging();
            //process.Kill();
        }

        private void CheckSettings()
        {
            if (Settings.Server == null || Settings.Wallet == null || Settings.Worker == null)
            {
                Setup setupWindow = new Setup();
                setupWindow.ShowDialog();
            }
        }
    }
}