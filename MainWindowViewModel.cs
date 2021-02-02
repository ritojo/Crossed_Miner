
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Crossed_Miner
{
    public class MainWindowViewModel : ObservableBase
    {
        private Settings settings;
        
        public MainWindowViewModel()
        {
            if (File.Exists("./settings.txt"))
            {
                using (StreamReader reader = new StreamReader("./settings.txt"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    settings = (Settings)serializer.Deserialize(reader, typeof(Settings));
                }
            }
            else
            {
                settings = new Settings();
            }
        }

        private void Exit()
        {

        }

        private Setup setupWindow;
        public Setup SetupWindow
        {
            get
            {
                if(setupWindow == null)
                {
                    setupWindow = new Setup(settings);
                }

                return setupWindow;
            }
        }

        private void Setup()
        {
            SetupWindow.Show();
        }

        private ICommand setupCommand;
        public  ICommand SetupCommand
        {
            get
            {
                if(setupCommand == null)
                {
                    setupCommand = new RelayCommand((x) => Setup());
                }

                return setupCommand;
            }
        }


        private ICommand closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if(closeCommand == null)
                {
                    closeCommand = new RelayCommand( (x) => Exit());
                }

                return closeCommand;
            }
        }
    }
}
