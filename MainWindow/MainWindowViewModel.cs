using System;
using System.Collections.Generic;
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
    }
}