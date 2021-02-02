using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Crossed_Miner
{
    static class Settings
    {
        public static String Server;
        public static String Worker;
        public static String Wallet;

        public static void SaveSettings()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("./settings.txt");
            file.WriteLine(Server);
            file.WriteLine(Worker);
            file.WriteLine(Wallet);
            file.Close();
        }

        public static void LoadSettings()
        {
            if (System.IO.File.Exists("./settings.txt"))
            {
                System.IO.StreamReader file = new System.IO.StreamReader("./settings.txt");
                Server = file.ReadLine();
                Worker = file.ReadLine();
                Wallet = file.ReadLine();
                file.Close();
            }
        }
    }
}
