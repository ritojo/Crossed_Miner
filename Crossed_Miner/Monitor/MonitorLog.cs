using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Crossed_Miner.Monitor
{
    class MonitorLog
    {
        //Instance Variables
        public Task task;
        public bool taskRunning = false;
        public Log RunLog;

        private Timer logTimer;

        private const string apiUri = "https://api.ethermine.org/";


        public MonitorLog()
        {
            //Spawn Task
            task = new Task(LoggerTask);
            task.Start();
            //Log testLog = JsonConvert.DeserializeObject<Log>(test);
            Console.WriteLine("Test");
        }

        public void StopLogging()
        {
            taskRunning = false;
        }

        private void LoggerTask()
        {
            taskRunning = true;

            // There is a limit of 100 requests every 15 minutes.
            // Data is cached for 2 minutes, so there is no
            // point in more frequent requests. We need to setup
            // a timer to handle this.
            logTimer = new Timer(119000);
            logTimer.Elapsed += TimerEvent;
            logTimer.AutoReset = true;
            logTimer.Enabled = true;

            while (taskRunning)
            {
                // Get Log Data
                string privateData = GetData();
                RunLog = JsonConvert.DeserializeObject<Log>(privateData);

                // There is a limit of 100 requests every 15 minutes.
                // Data is cached for 2 minutes, so there is no
                // point in more frequent requests. We need to setup
                // a timer to handle this.
                Task.Delay(200);
            }
        }

        private static void TimerEvent(Object source, ElapsedEventArgs e)
        {

        }

        private string GetData()
        {
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:4067/summary");
            //try
            //{
            //    WebResponse response = request.GetResponse();
            //    using (Stream responseStream = response.GetResponseStream())
            //    {
            //        StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
            //        return reader.ReadToEnd();
            //    }
            //}
            //catch (WebException ex)
            //{
            //    WebResponse errorResponse = ex.Response;
            //    using (Stream responseStream = errorResponse.GetResponseStream())
            //    {
            //        StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
            //        String errorText = reader.ReadToEnd();
            //        // log errorText
            //    }
            //    throw;
            //}
            return String.Empty;
        }
    }

    #region PoolStats
    internal class PoolStatsResponse
    {

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    internal class MinedBlock
    {
        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("miner")]
        public string Miner { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }
    }

    internal class PoolStats
    {
        [JsonProperty("hashRate")]
        public double HashRate { get; set; }

        [JsonProperty("miners")]
        public int Miners { get; set; }

        [JsonProperty("workers")]
        public int Workers { get; set; }

        [JsonProperty("blocksPerHour")]
        public double BlocksPerHour { get; set; }
    }

    internal class Price
    {
        [JsonProperty("usd")]
        public double Usd { get; set; }

        [JsonProperty("btc")]
        public double Btc { get; set; }
    }

    internal class Data
    {
        [JsonProperty("topMiners")]
        public IList<object> TopMiners { get; set; }

        [JsonProperty("minedBlocks")]
        public IList<MinedBlock> MinedBlocks { get; set; }

        [JsonProperty("poolStats")]
        public PoolStats PoolStats { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }
    }
    #endregion

    #region BlocksHistory
    internal class BlocksHistoryResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public IList<Datum> Data { get; set; }
    }

    internal class Datum
    {
        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("nbrBlocks")]
        public int NbrBlocks { get; set; }

        [JsonProperty("difficulty")]
        public object Difficulty { get; set; }
    }
    #endregion

    #region NetworkStats
    internal class NetworkStatsResponse
    {

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public Data NetStatsData { get; set; }
    }

    internal class NetStatsData
    {
        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("blockTime")]
        public double BlockTime { get; set; }

        [JsonProperty("difficulty")]
        public long Difficulty { get; set; }

        [JsonProperty("hashrate")]
        public long Hashrate { get; set; }

        [JsonProperty("usd")]
        public double Usd { get; set; }

        [JsonProperty("btc")]
        public double Btc { get; set; }
    }
    #endregion

    #region ServerHistory
    internal class ServerHistoryResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public IList<ServerHistoryDatum> Data { get; set; }
    }
    internal class ServerHistoryDatum
    {
        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("hashrate")]
        public double Hashrate { get; set; }
    }
    #endregion
}
