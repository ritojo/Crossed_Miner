using Gstc.Collections.Observable;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
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

        private ObservableList<GpuStatsViewModel> gpuList = null;
        public ObservableList<GpuStatsViewModel> GpuList
        {
            get
            {
                if (gpuList == null)
                {
                    gpuList = new ObservableList<GpuStatsViewModel>();
                }

                return gpuList;
            }
            set
            {
                if (gpuList != value)
                {
                    gpuList = value;
                    OnPropertyChanged("GpuList");
                }
            }
        }

        private GpuStatsViewModel currentGpuStatsViewModel = null;
        public GpuStatsViewModel CurrentGpuStatsViewModel
        {
            get
            {
                if (currentGpuStatsViewModel == null)
                {
                    currentGpuStatsViewModel = new GpuStatsViewModel();
                }

                return currentGpuStatsViewModel;
            }
            set
            {
                if (currentGpuStatsViewModel != value)
                {
                    currentGpuStatsViewModel = value;
                    OnPropertyChanged("CurrentGpuStatsViewModel");
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
                MessageBoxResult result =  MessageBox.Show(Application.Current.MainWindow, "T-Rex.exe not found. Do you want to download it?", "You Suck", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch(result)
                {
                    case MessageBoxResult.Yes:
                        // Download T-Rex
                        using (WebClient wc = new WebClient())
                        {
                            wc.UseDefaultCredentials = true;
                            wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
                            wc.DownloadFileAsync( new System.Uri("https://github.com/trexminer/T-Rex/releases/download/0.19.9/t-rex-0.19.9-win-cuda11.1.zip"), "./t-rex.zip");
                        }

                        break;

                    case MessageBoxResult.No:
                    default:
                        MessageBox.Show(Application.Current.MainWindow, "No mining for you then!", "You Really Suck", MessageBoxButton.OK, MessageBoxImage.Error);
                        isMining = false;
                        break;
                }
            }

            CheckSettings(miningConfig); //Pass as dependency for unit testing purposes if we want

            //processStartInfo.Arguments = "-a ethash --api-bind-telnet 0 --api-bind-http 127.0.0.1:4067 -o stratum+tcp://" + Settings.Server + ":4444 -u " + Settings.Wallet + " -p x -w " + Settings.Worker;
            personalLog = new PersonalLog();
            personalLog.DataUpdatedEvent += pl_UpdateUIWithNewData;
            //process = Process.Start(processStartInfo);
        }

        private void pl_UpdateUIWithNewData(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            int numGpus = personalLog.RunLog.GPUTotal;

            if (GpuList.Count != numGpus)
            {
                //Pull in all the GPU info
                foreach (GPUS gpu in personalLog.RunLog.GPUs)
                {
                    //Might not be the best way to do this, but if the GPU list changes at all, dump our local list and reinitialize it
                    GpuList.Clear();
                    GpuList.Add(new GpuStatsViewModel());
                }
            }

            for (int i = 0; i < numGpus; i++)
            {
                GPUS gpuData               = personalLog.RunLog.GPUs[i];
                GpuList[i].Name            = $"{gpuData.Vendor} {gpuData.Name}";
                GpuList[i].DeviceID        = gpuData.DeviceID;
                GpuList[i].Efficiency      = gpuData.Efficiency;
                GpuList[i].PowerDraw       = gpuData.Power;
                GpuList[i].Temperature     = gpuData.Temperature;
                GpuList[i].Hashrate        = gpuData.Hashrate;
                GpuList[i].Intensity       = gpuData.Intensity;
                GpuList[i].FanSpeedPercent = gpuData.FanSpeed;

                StatByGPU gpuStats         = personalLog.RunLog.StatByGPUs[i];
                GpuList[i].AcceptedCount   = gpuStats.AcceptedCount;
                GpuList[i].RejectedCount   = gpuStats.RejectedCount;
                GpuList[i].SolvedCount     = gpuStats.SolvedCount;
            }
        }

        // Event to track the progress
        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            int Value = e.ProgressPercentage;
            Console.WriteLine(e.ProgressPercentage);
        }

        // Download Completed Event
        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Unzip t-rex.exe
            using (ZipArchive archive = ZipFile.OpenRead("./t-rex.zip"))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if(entry.FullName.Contains("t-rex.exe"))
                    entry.ExtractToFile(Path.Combine(".", entry.FullName));
                }
            }

            //Delete Zip
            File.Delete("./t-rex.zip");
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