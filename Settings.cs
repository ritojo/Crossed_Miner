using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Crossed_Miner
{
    public class Settings
    {
        public const string FileName = @"./settings.txt";

        public String Server;
        public String Worker;
        public String Wallet;

        public Settings()
        {

        }

        public void SaveSettings()
        {
            using (StreamWriter file = File.CreateText(FileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, this);
            }
        }
    }
}
