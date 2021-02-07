using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossed_Miner
{
    public class GpuStatsViewModel : ObservableBase
    {
        public GpuStatsViewModel()
        {

        }

        private string name = string.Empty;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private int deviceID = 0;
        public int DeviceID
        {
            get
            {
                return deviceID;
            }
            set
            {
                if (deviceID != value)
                {
                    deviceID = value;
                    OnPropertyChanged("DeviceID");
                }
            }
        }

        private string efficiency = string.Empty;
        public string Efficiency
        {
            get
            {
                return efficiency;
            }
            set
            {
                if (efficiency != value)
                {
                    efficiency = value;
                    OnPropertyChanged("Efficiency");
                }
            }
        }

        private int powerDraw = 0;
        public int PowerDraw
        {
            get
            {
                return powerDraw;
            }
            set
            {
                if (powerDraw != value)
                {
                    powerDraw = value;
                    OnPropertyChanged("PowerDraw");
                }
            }
        }

        private int temperature = 0;
        public int Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                if (temperature != value)
                {
                    temperature = value;
                    OnPropertyChanged("Temperature");
                }
            }
        }

        private int hashrate = 0;
        public int Hashrate
        {
            get
            {
                return hashrate;
            }
            set
            {
                if (hashrate != value)
                {
                    hashrate = value;
                    OnPropertyChanged("Hashrate");
                }
            }
        }

        private double intensity = 0.0;
        public double Intensity
        {
            get
            {
                return intensity;
            }
            set
            {
                if (intensity != value)
                {
                    intensity = value;
                    OnPropertyChanged("Intensity");
                }
            }
        }

        private int acceptedCount = 0;
        public int AcceptedCount
        {
            get
            {
                return acceptedCount;
            }
            set
            {
                if (acceptedCount != value)
                {
                    acceptedCount = value;
                    OnPropertyChanged("AcceptedCount");
                }
            }
        }

        private int rejectedCount = 0;
        public int RejectedCount
        {
            get
            {
                return rejectedCount;
            }
            set
            {
                if (rejectedCount != value)
                {
                    rejectedCount = value;
                    OnPropertyChanged("RejectedCount");
                }
            }
        }

        private int solvedCount = 0;
        public int SolvedCount
        {
            get
            {
                return solvedCount;
            }
            set
            {
                if (solvedCount != value)
                {
                    solvedCount = value;
                    OnPropertyChanged("SolvedCount");
                }
            }
        }

        private int fanSpeedPercent = 0;
        public int FanSpeedPercent
        {
            get
            {
                return fanSpeedPercent;
            }
            set
            {
                if (fanSpeedPercent != value)
                {
                    fanSpeedPercent = value;
                    OnPropertyChanged("FanSpeedPercent");
                }
            }
        }
    }
}
