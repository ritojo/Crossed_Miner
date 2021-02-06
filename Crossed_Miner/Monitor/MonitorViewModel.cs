using Crossed_Miner.Monitor;

namespace Crossed_Miner
{
    public class MonitorViewModel : ObservableBase
    {
        private MiningConfig miningConfig;
        private MonitorLog monitorLog;

        public MonitorViewModel(MiningConfig config)
        {
            miningConfig = config;
            monitorLog = new MonitorLog(miningConfig);
        }
    }
}
