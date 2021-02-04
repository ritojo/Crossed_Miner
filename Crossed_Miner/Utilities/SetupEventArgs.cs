using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossed_Miner
{
    public class SetupEventArgs : EventArgs
    {
        public string Server { get; }
        public string Worker { get; }
        public string WalletID { get; }

        public SetupEventArgs(string server, string worker, string walletID)
        {
            Server = server;
            Worker = worker;
            WalletID = walletID;
        }
    }
}
