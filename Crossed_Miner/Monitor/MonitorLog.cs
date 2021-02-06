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
        private MiningConfig monitorConfig;

        private const string apiUri = "https://api.ethermine.org";

        // Pool Commands
        private const string poolStatsCommand = "/poolStats";
        private const string blocksHistoryCommand = "/blocks/history";
        private const string networkStatsCommand = "/networkStats";
        private const string serverHistoryCommand = "/servers/history";

        // Miner Commands
        private string minerDashboardCommand;
        private string minerHistoryCommand;
        private string minerPayoutsCommand;
        private string minerRoundsCommand;
        private string minerSettingsCommand;
        private string minerStatisticsCommand;

        // Worker Commands
        private string workerStatisticsCommand;
        private string workerIndividualHistoricalStatisticsCommand;
        private string workerIndividualStatisticsCommand;
        private string workermonitorCommand;

        public MonitorLog(MiningConfig config)
        {
            monitorConfig = config;

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

        private void BuildMinerCommands()
        {
            if (monitorConfig.WalletID != null)
            {
                minerDashboardCommand = "/miner/" + monitorConfig.WalletID + "/dashboard";
                minerHistoryCommand = "/miner/" + monitorConfig.WalletID + "/history";
                minerPayoutsCommand = "/miner/" + monitorConfig.WalletID + "/payouts";
                minerRoundsCommand = "/miner/" + monitorConfig.WalletID + "/rounds";
                minerSettingsCommand = "/miner/" + monitorConfig.WalletID + "/settings";
                minerStatisticsCommand = "/miner/" + monitorConfig.WalletID + "/currentStats";
            }
        }

        private void BuildWorkerCommands()
        {

        }
    }

    #region PoolStats
    internal class PoolStatsResponse
    {

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public PoolStatsData Data { get; set; }
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

    internal class PoolStatsData
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
        public IList<BlockHistoryDatum> Data { get; set; }
    }

    internal class BlockHistoryDatum
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
        public NetStatsData Data { get; set; }
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

    #region MinerDashboard
    internal class MinerDashboardResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public MinerDashboardData Data { get; set; }
    }

    internal class Statistic
    {
        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("reportedHashrate")]
        public int ReportedHashrate { get; set; }

        [JsonProperty("currentHashrate")]
        public double CurrentHashrate { get; set; }

        [JsonProperty("validShares")]
        public int ValidShares { get; set; }

        [JsonProperty("invalidShares")]
        public int InvalidShares { get; set; }

        [JsonProperty("staleShares")]
        public int StaleShares { get; set; }

        [JsonProperty("activeWorkers")]
        public int ActiveWorkers { get; set; }
    }

    internal class Worker
    {

        [JsonProperty("worker")]
        public string WorkerName { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("lastSeen")]
        public int LastSeen { get; set; }

        [JsonProperty("reportedHashrate")]
        public int ReportedHashrate { get; set; }

        [JsonProperty("currentHashrate")]
        public double CurrentHashrate { get; set; }

        [JsonProperty("validShares")]
        public int ValidShares { get; set; }

        [JsonProperty("invalidShares")]
        public int InvalidShares { get; set; }

        [JsonProperty("staleShares")]
        public int StaleShares { get; set; }
    }

    internal class CurrentStatistics
    {

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("lastSeen")]
        public int LastSeen { get; set; }

        [JsonProperty("reportedHashrate")]
        public int ReportedHashrate { get; set; }

        [JsonProperty("currentHashrate")]
        public double CurrentHashrate { get; set; }

        [JsonProperty("validShares")]
        public int ValidShares { get; set; }

        [JsonProperty("invalidShares")]
        public int InvalidShares { get; set; }

        [JsonProperty("staleShares")]
        public int StaleShares { get; set; }

        [JsonProperty("activeWorkers")]
        public int ActiveWorkers { get; set; }

        [JsonProperty("unpaid")]
        public long Unpaid { get; set; }
    }

    internal class Settings
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("monitor")]
        public int Monitor { get; set; }

        [JsonProperty("minPayout")]
        public long MinPayout { get; set; }
    }

    internal class MinerDashboardData
    {
        [JsonProperty("statistics")]
        public IList<Statistic> Statistics { get; set; }

        [JsonProperty("workers")]
        public IList<Worker> Workers { get; set; }

        [JsonProperty("currentStatistics")]
        public CurrentStatistics CurrentStatistics { get; set; }

        [JsonProperty("settings")]
        public Settings Settings { get; set; }
    }
    #endregion

    #region MinerHistory
    internal class MinerHistoryResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public IList<MinerHistoryDatum> Data { get; set; }
    }

    internal class MinerHistoryDatum
    {
        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("reportedHashrate")]
        public int ReportedHashrate { get; set; }

        [JsonProperty("currentHashrate")]
        public double CurrentHashrate { get; set; }

        [JsonProperty("validShares")]
        public int ValidShares { get; set; }

        [JsonProperty("invalidShares")]
        public int InvalidShares { get; set; }

        [JsonProperty("staleShares")]
        public int StaleShares { get; set; }

        [JsonProperty("averageHashrate")]
        public double AverageHashrate { get; set; }

        [JsonProperty("activeWorkers")]
        public int ActiveWorkers { get; set; }
    }
    #endregion

    #region MinerPayouts
    internal class MinerPayoutsResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public IList<MinerPayoutsDatum> Data { get; set; }
    }

    internal class MinerPayoutsDatum
    {
        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("end")]
        public int End { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("txHash")]
        public string TxHash { get; set; }

        [JsonProperty("paidOn")]
        public int PaidOn { get; set; }
    }
    #endregion

    #region MinerRounds
    internal class MinerRoundsResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public IList<MinerRoundsDatum> Data { get; set; }
    }

    internal class MinerRoundsDatum
    {
        [JsonProperty("block")]
        public int Block { get; set; }

        [JsonProperty("amount")]
        public object Amount { get; set; }
    }
    #endregion

    #region MinerSettings
    internal class MinerSettingsResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public MinerSettingsData Data { get; set; }
    }

    internal class MinerSettingsData
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("monitor")]
        public int Monitor { get; set; }

        [JsonProperty("minPayout")]
        public long MinPayout { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }
    }
    #endregion

    #region MinerCurrentStats
    internal class MinerCurrentStatsResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public MinerCurrentStatsData Data { get; set; }
    }

    internal class MinerCurrentStatsData
    {
        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("lastSeen")]
        public int LastSeen { get; set; }

        [JsonProperty("reportedHashrate")]
        public int ReportedHashrate { get; set; }

        [JsonProperty("currentHashrate")]
        public double CurrentHashrate { get; set; }

        [JsonProperty("validShares")]
        public int ValidShares { get; set; }

        [JsonProperty("invalidShares")]
        public int InvalidShares { get; set; }

        [JsonProperty("staleShares")]
        public int StaleShares { get; set; }

        [JsonProperty("averageHashrate")]
        public double AverageHashrate { get; set; }

        [JsonProperty("activeWorkers")]
        public int ActiveWorkers { get; set; }

        [JsonProperty("unpaid")]
        public long Unpaid { get; set; }

        [JsonProperty("unconfirmed")]
        public object Unconfirmed { get; set; }

        [JsonProperty("coinsPerMin")]
        public double CoinsPerMin { get; set; }

        [JsonProperty("usdPerMin")]
        public double UsdPerMin { get; set; }

        [JsonProperty("btcPerMin")]
        public double BtcPerMin { get; set; }
    }
    #endregion
}
