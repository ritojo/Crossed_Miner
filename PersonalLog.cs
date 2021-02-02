using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crossed_Miner
{
    class PersonalLog
    {
        //Instance Variables
        public Task task;
        public bool taskRunning = false;
        public Log RunLog;
        public string test = @"{""accepted_count"":85,""active_pool"":{""difficulty"":""4.00 G"",""ping"":49,""retries"":0,""url"":""stratum+tcp://us1.ethermine.org:4444"",""user"":""0x71f92728888aC84a0bdA64dDE7dd538e20d07E42""},""algorithm"":""ethash"",""api"":""3.3"",""build_date"":""Jan 19 2021 23:35:26"",""coin"":"""",""cuda"":""11.10"",""description"":""T-Rex NVIDIA GPU miner"",""difficulty"":0.99392999999999998,""driver"":""461.09"",""gpu_total"":1,""gpus"":[{""dag_build_mode"":0,""device_id"":0,""efficiency"":""369kH/W"",""fan_speed"":100,""gpu_id"":0,""gpu_user_id"":0,""hashrate"":75321106,""hashrate_day"":80393683,""hashrate_hour"":75997617,""hashrate_instant"":75144238,""hashrate_minute"":75285363,""intensity"":25.0,""low_load"":false,""mtweak"":0,""name"":""RTX 3080"",""power"":205,""power_avr"":204,""temperature"":58,""uuid"":""c342a09b60a8eeef9c47110bbfa5b4f4"",""vendor"":""MSI""}],""hashrate"":75321106,""hashrate_day"":80393683,""hashrate_hour"":75997617,""hashrate_minute"":75285363,""name"":""t-rex"",""os"":""win"",""rejected_count"":0,""revision"":""4fb1aa6877f6"",""sharerate"":1.1839999999999999,""sharerate_average"":1.028,""solved_count"":0,""stat_by_gpu"":[{""accepted_count"":85,""rejected_count"":0,""solved_count"":0}],""success"":1,""ts"":1612294640,""uptime"":5036,""version"":""0.19.9"",""watchdog_stat"":{""built_in"":true,""startup_ts"":1612289600808645,""total_restarts"":0,""uptime"":5039,""wd_version"":""0.19.9""}}";

        public PersonalLog()
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

            while(taskRunning)
            {
                // Get Log Data
                string privateData = GetData();
                RunLog = JsonConvert.DeserializeObject<Log>(privateData);

                // Delay
                Task.Delay(200);
            }
        }

        private string GetData()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:4067/summary");
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
    }

    class Log
    {
        //Instance Variables
        [JsonProperty("accepted_count")]
        public int AcceptedCount { get; set; }

        [JsonProperty("active_pool")]
        public Pool ActivePool { get; set; }

        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }

        [JsonProperty("api")]
        public string API { get; set; }

        [JsonProperty("build_date")]
        public string BuildDate { get; set; }

        [JsonProperty("coin")]
        public string Coin { get; set; }

        [JsonProperty("cuda")]
        public string Cuda { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("difficulty")]
        public double Difficulty { get; set; }

        [JsonProperty("driver")]
        public string Driver { get; set; }

        [JsonProperty("gpu_total")]
        public int GPUTotal { get; set; }

        [JsonProperty("gpus")]
        public List<GPUS> GPUs { get; set; }

        [JsonProperty("hashrate")]
        public int Hashrate { get; set; }

        [JsonProperty("hashrate_day")]
        public int HashrateDay { get; set; }

        [JsonProperty("hashrate_hour")]
        public int HashrateHour { get; set; }

        [JsonProperty("hashrate_minute")]
        public int HashrateMinute { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("os")]
        public string OS { get; set; }

        [JsonProperty("rejected_count")]
        public int RejectedCount { get; set; }

        [JsonProperty("revision")]
        public string Revision { get; set; }

        [JsonProperty("sharerate")]
        public double Sharerate { get; set; }

        [JsonProperty("sharerate_average")]
        public double SharerateAverage { get; set; }

        [JsonProperty("solved_count")]
        public int SolvedCount { get; set; }

        [JsonProperty("stat_by_gpu")]
        public List<StatByGPU> StatByGPUs { get; set; }

        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("ts")]
        public int TS { get; set; }

        [JsonProperty("uptime")]
        public int Uptime { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("watchdog_stat")]
        public WatchdogStat WatchdogStats { get; set; }
    }
    class Pool
    {
        // Instance Variables
        [JsonProperty("difficulty")]
        public string Difficulty { get; set; }

        [JsonProperty("ping")]
        public int Ping { get; set; }

        [JsonProperty("retries")]
        public int Retries { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }
    }
    class GPUS
    {
        [JsonProperty("dag_build_mode")]
        public int DagBuildMode { get; set; }

        [JsonProperty("device_id")]
        public int DeviceID { get; set; }

        [JsonProperty("efficiency")]
        public string Efficiency { get; set; }

        [JsonProperty("fan_speed")]
        public int FanSpeed { get; set; }

        [JsonProperty("gpu_id")]
        public int GPUID { get; set; }

        [JsonProperty("gpu_user_id")]
        public int GPUUserID { get; set; }

        [JsonProperty("hashrate")]
        public int Hashrate { get; set; }

        [JsonProperty("hashrate_day")]
        public int HashrateDay { get; set; }

        [JsonProperty("hashrate_hour")]
        public int HashrateHour { get; set; }

        [JsonProperty("hashrate_instant")]
        public int HashrateInstant { get; set; }

        [JsonProperty("hashrate_minute")]
        public int HashrateMinute { get; set; }

        [JsonProperty("intensity")]
        public double Intensity { get; set; }

        [JsonProperty("low_load")]
        public bool LowLoad { get; set; }

        [JsonProperty("mtweak")]
        public int Mtweak { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("power")]
        public int Power { get; set; }

        [JsonProperty("power_avr")]
        public int PowerAVR { get; set; }

        [JsonProperty("temperature")]
        public int Temperature { get; set; }

        [JsonProperty("uuid")]
        public string UUID { get; set; }

        [JsonProperty("vendor")]
        public string Vendor { get; set; }
    }
    class StatByGPU
    {
        [JsonProperty("accepted_count")]
        public int AcceptedCount { get; set; }

        [JsonProperty("rejected_count")]
        public int RejectedCount { get; set; }

        [JsonProperty("solved_count")]
        public int SolvedCount { get; set; }
    }
    class WatchdogStat
    {
        [JsonProperty("built_in")]
        public bool BuiltIn { get; set; }

        [JsonProperty("startup_ts")]
        public long StartupTS { get; set; }

        [JsonProperty("total_restarts")]
        public int TotalRestarts { get; set; }

        [JsonProperty("uptime")]
        public int Uptime { get; set; }

        [JsonProperty("wd_version")]
        public string WDVersion { get; set; }
    }
}
