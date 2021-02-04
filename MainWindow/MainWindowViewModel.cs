using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private MiningConfig miningConfig = null;
        public MiningConfig MiningConfig
        {
            get
            {
                if (miningConfig == null)
                {
                    try
                    {
                        using (StreamReader file = File.OpenText(Settings.Default.MiningConfigFile))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            miningConfig = (MiningConfig)serializer.Deserialize(file, typeof(MiningConfig));
                        }
                    }
                    catch (Exception e)
                    {
                        //TODO: use logger instead
                        MessageBox.Show("File " + Settings.Default.MiningConfigFile + " not found");
                    }
                }

                return miningConfig;
            }
        }

        private SetupViewModel setupViewModel = null;
        public SetupViewModel SetupViewModel
        {
            get
            {
                if (setupViewModel == null)
                {
                    setupViewModel = new SetupViewModel();
                    setupViewModel.Initialize(MiningConfig);
                    setupViewModel.SavedSettingsEvent += SetupSettingsSavedHandler;
                    setupViewModel.CancelSettingsEvent += SetupSettingsNotSavedHandler;
                }

                return setupViewModel;
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
            MiningConfig.Server = e.Server;
            MiningConfig.Worker = e.Worker;
            MiningConfig.WalletID = e.WalletID;

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
            SetupViewModel.Initialize(MiningConfig);
            IsSetupDisplayed = false;
        }

        private void WriteMiningConfigToFile()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(Settings.Default.MiningConfigFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, MiningConfig, typeof(MiningConfig));
            }
        }
    }
}