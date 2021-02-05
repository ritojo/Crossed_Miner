using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Diagnostics;
using System.IO;
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
        private MiningConfig miningConfig;

        public MinerViewModel(MiningConfig config)
        {
            miningConfig = config;

            processStartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = exeName,
                WindowStyle = ProcessWindowStyle.Hidden
            };
        }

        private PlotModel miningPlot = null;
        public PlotModel MiningPlot
        {
            get
            {
                if (miningPlot == null)
                {
                    miningPlot = new PlotModel() { Title = "TestPlot" };
                    miningPlot.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
                    miningPlot.InvalidatePlot(true);
                }

                return miningPlot;
            }
            set
            {
                miningPlot = value;
                OnPropertyChanged("MiningPlot");
            }
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
                IsMining = false;
                return;
            }

            CheckSettings(miningConfig); //Pass as dependency for unit testing purposes if we want

            //processStartInfo.Arguments = "-a ethash --api-bind-telnet 0 --api-bind-http 127.0.0.1:4067 -o stratum+tcp://" + Settings.Server + ":4444 -u " + Settings.Wallet + " -p x -w " + Settings.Worker;
            personalLog = new PersonalLog();
            //process = Process.Start(processStartInfo);
        }

        private void StopMining()
        {
            personalLog.StopLogging();
            //process.Kill();
        }

        private bool isSetupDisplayed = false;
        public bool IsSetupDisplayed
        {
            get
            {
                return isSetupDisplayed;
            }
            set
            {
                if (isSetupDisplayed != value)
                {
                    isSetupDisplayed = value;
                    OnPropertyChanged("IsSetupDisplayed");
                }
            }
        }

        private void CheckSettings(MiningConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.Server) || (string.IsNullOrWhiteSpace(config.Worker)) || (string.IsNullOrWhiteSpace(config.WalletID)))
            {
                IsSetupDisplayed = true;
            }
        }

        private SetupViewModel currentMinerSetupViewModel = null;
        public SetupViewModel CurrentMinerSetupViewModel
        {
            get
            {
                if (currentMinerSetupViewModel == null)
                {
                    currentMinerSetupViewModel = new SetupViewModel();
                    currentMinerSetupViewModel.Initialize(miningConfig);
                    currentMinerSetupViewModel.SavedSettingsEvent += SetupSettingsSavedHandler;
                    currentMinerSetupViewModel.CancelSettingsEvent += SetupSettingsNotSavedHandler;
                }

                return currentMinerSetupViewModel;
            }
        }

        /// <summary>
        /// This function is subscribed to the SetupViewModel event. When the event is invoked, this function will pull in Setup info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetupSettingsSavedHandler(object sender, SetupEventArgs e)
        {
            //Copy info over from the event in the SetupViewModel
            miningConfig.Server = e.Server;
            miningConfig.Worker = e.Worker;
            miningConfig.WalletID = e.WalletID;

            //Close the window
            IsSetupDisplayed = false;

            WriteMiningConfigToFile();
        }

        /// <summary>
        /// Revert settings displayed back to the config and close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetupSettingsNotSavedHandler(object sender, EventArgs e)
        {
            CurrentMinerSetupViewModel.Initialize(miningConfig);
            IsSetupDisplayed = false;
        }

        private void WriteMiningConfigToFile()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(Settings.Default.MiningConfigFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, miningConfig, typeof(MiningConfig));
            }
        }
    }
}