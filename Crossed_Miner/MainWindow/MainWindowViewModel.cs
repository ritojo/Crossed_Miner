using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Crossed_Miner
{
    public class MainWindowViewModel : ObservableBase
    {
        public MainWindowViewModel()
        {

        }

        private ICommand displaySetupCommand = null;
        public ICommand DisplaySetupCommand
        {
            get
            {
                if (displaySetupCommand == null)
                {
                    displaySetupCommand = new RelayCommand(param => DisplaySetup());
                }

                return displaySetupCommand;
            }
        }

        private void DisplaySetup()
        {
            IsSetupDisplayed = true;
        }

        private ICommand closeCommand = null;
        public ICommand CloseCommand
        {
            get
            {
                if (closeCommand == null)
                {
                    closeCommand = new RelayCommand(param => Exit());
                }

                return closeCommand;
            }
        }

        private void Exit()
        {
            Application.Current.MainWindow.Close();
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

        private MiningConfig currentMiningConfig = null;
        public MiningConfig CurrentMiningConfig
        {
            get
            {
                if (currentMiningConfig == null)
                {
                    try
                    {
                        using (StreamReader file = File.OpenText(Settings.Default.MiningConfigFile))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            currentMiningConfig = (MiningConfig)serializer.Deserialize(file, typeof(MiningConfig));
                        }
                    }
                    catch (Exception e)
                    {
                        //TODO: use logger instead
                        MessageBox.Show("File " + Settings.Default.MiningConfigFile + " not found");
                    }
                }

                return currentMiningConfig;
            }
        }

        private SetupViewModel currentSetupViewModel = null;
        public SetupViewModel CurrentSetupViewModel
        {
            get
            {
                if (currentSetupViewModel == null)
                {
                    currentSetupViewModel = new SetupViewModel();
                    currentSetupViewModel.Initialize(CurrentMiningConfig);
                    currentSetupViewModel.SavedSettingsEvent += SetupSettingsSavedHandler;
                    currentSetupViewModel.CancelSettingsEvent += SetupSettingsNotSavedHandler;
                }

                return currentSetupViewModel;
            }
        }

        private MinerViewModel currentMinerViewModel = null;
        public MinerViewModel CurrentMinerViewModel
        {
            get
            {
                if (currentMinerViewModel == null)
                {
                    currentMinerViewModel = new MinerViewModel(CurrentMiningConfig);
                }

                return currentMinerViewModel;
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
            CurrentMiningConfig.Server = e.Server;
            CurrentMiningConfig.Worker = e.Worker;
            CurrentMiningConfig.WalletID = e.WalletID;

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
            CurrentSetupViewModel.Initialize(CurrentMiningConfig);
            IsSetupDisplayed = false;
        }

        private void WriteMiningConfigToFile()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(Settings.Default.MiningConfigFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, CurrentMiningConfig, typeof(MiningConfig));
            }
        }
    }
}