using Gstc.Collections.Observable;
using System;
using System.Windows.Input;

namespace Crossed_Miner
{
    public class SetupViewModel : ObservableBase
    {
        public SetupViewModel()
        {

        }

        public void Initialize(MiningConfig config)
        {
            ChosenServer = config.Server;
            Worker = config.Worker;
            WalletID = config.WalletID;
        }

        private ObservableList<string> serverList = null;
        public ObservableList<string> ServerList
        {
            get
            {
                if (serverList == null)
                {
                    //TODO put this in config that's parsed from JSON and copy to here
                    serverList = new ObservableList<string>()
                    {
                        "asia1.ethermine.org",
                        "eu1.ethermine.org",
                        "us1.ethermine.org",
                        "us2.ethermine.org"
                    };
                }

                return serverList;
            }
        }

        private string chosenServer = string.Empty;
        public string ChosenServer
        {
            get
            {
                if (chosenServer == string.Empty)
                {
                    chosenServer = ServerList[0];
                }

                return chosenServer;
            }
            set
            { 
                if (chosenServer != value)
                {
                    chosenServer = value;
                    OnPropertyChanged("ChosenServer");
                }
            }
        }

        private string worker = string.Empty;
        public string Worker
        {
            get
            {
                return worker;
            }
            set
            {
                if (worker != value)
                {
                    worker = value;
                    OnPropertyChanged("Worker");
                }
            }
        }

        private string walletID = string.Empty;
        public string WalletID
        {
            get
            {
                return walletID;
            }
            set
            {
                if (walletID != value)
                {
                    walletID = value;
                    OnPropertyChanged("WalletID");
                }
            }
        }

        private ICommand saveSettingsCommand = null;
        public ICommand SaveSettingsCommand
        {
            get
            {
                if (saveSettingsCommand == null)
                {
                    saveSettingsCommand = new RelayCommand(param => SaveSettings());
                }

                return saveSettingsCommand;
            }
        }

        private void SaveSettings()
        {
            //Error check what the user put in
            if ((ChosenServer != null) && (Worker != null) && (WalletID != null))
            {
                //Invoke the event to kick the info back to the event subscriber
                OnSaveSettings(new SetupEventArgs(ChosenServer, Worker, WalletID));
            }
        }

        public event EventHandler<SetupEventArgs> SavedSettingsEvent;
        private void OnSaveSettings(SetupEventArgs e)
        {
            SavedSettingsEvent?.Invoke(this, e);
        }

        private ICommand cancelSettingsCommand = null;
        public ICommand CancelSettingsCommand
        {
            get
            {
                if (cancelSettingsCommand == null)
                {
                    cancelSettingsCommand = new RelayCommand(param => CancelSettings());
                }

                return cancelSettingsCommand;
            }
        }

        private void CancelSettings()
        {
            OnCancelSettings(EventArgs.Empty);
        }

        public event EventHandler<EventArgs> CancelSettingsEvent;
        private void OnCancelSettings(EventArgs e)
        {
            CancelSettingsEvent?.Invoke(this, e);
        }
    }
}
