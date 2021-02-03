using Gstc.Collections.Observable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Crossed_Miner
{
    public class SetupViewModel : ObservableBase
    {
        public SetupViewModel()
        {

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
            //if (serverComboBox.SelectedItem != null)
            //{
            //    Settings.Server = serverComboBox.SelectedItem.ToString();
            //}
            //
            //if (walletTextBox.Text != null)
            //{
            //    Settings.Wallet = walletTextBox.Text;
            //}
            //
            //if (workerTextBox.Text != null)
            //{
            //    Settings.Worker = workerTextBox.Text;
            //}
            //
            //Settings.SaveSettings();
            //this.Close();
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
            //TODO: close out view
        }
    }
}
