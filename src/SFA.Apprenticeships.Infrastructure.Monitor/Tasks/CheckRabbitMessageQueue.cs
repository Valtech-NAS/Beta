namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using EasyNetQ;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NLog;
    using RestSharp;
    using RestSharp.Deserializers;
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;

    public class CheckRabbitMessageQueue : IMonitorTask
    {
        private readonly IConfigurationManager _configurationManager;
        private const string MonitorRabbitAppSetting = "Monitor.Rabbit";
        private const string MonitorRelkAppSetting = "Monitor.Relk";
        private const string MonitorRabbitExpectedNodeCountAppSetting = "Monitor.Rabbit.ExpectedNodeCount";
        private const string MonitorRelkExpectedNodeCountAppSetting = "Monitor.Relk.ExpectedNodeCount";
        private const int RabbitApiPort = 15672;
        private const string RabbitNodesApiPath = "api/nodes";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IBus _rabbitBus;

        public CheckRabbitMessageQueue(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public string TaskName
        {
            get { return "Check rabbit message queues"; }
        }

        public void Run()
        {
            var rabbitConnectionString = _configurationManager.GetAppSetting(MonitorRabbitAppSetting);
            var relkConnectionString = _configurationManager.GetAppSetting(MonitorRelkAppSetting);
            var rabbitExpectedNodeCount =
                _configurationManager.GetAppSetting<int>(MonitorRabbitExpectedNodeCountAppSetting);
            var relkExpectedNodeCount =
                _configurationManager.GetAppSetting<int>(MonitorRelkExpectedNodeCountAppSetting);

            CheckRabbitHealth(rabbitConnectionString, rabbitExpectedNodeCount);
            CheckRabbitHealth(relkConnectionString, relkExpectedNodeCount);
        }

        private void CheckRabbitHealth(string connectionString, int expectedNodecount)
        {
            CheckRabbitQueueConnectivity(connectionString);
            CheckRabbitNodeCount(connectionString, expectedNodecount);
        }

        private void CheckRabbitQueueConnectivity(string connectionString)
        {
            try
            {
                using (_rabbitBus = RabbitHutch.CreateBus(connectionString))
                {
                    _rabbitBus.Publish(new MonitorMessage());    
                }
            }
            catch (Exception ex)
            {
                var host = connectionString.Split(new[] { ';' }).First();
                LogError(host, ex);
            }
        }

        private void CheckRabbitNodeCount(string connectionString, int expectedNodeCount)
        {
            try
            {
                var hostIp = connectionString.Split(new[] {';'}).First().Split(new[] {'='}).Last();
                var restClient = GetRestClient();
                var user = GetUserFrom(connectionString);
                var password = GetPasswordFrom(connectionString);
                var requestUrl = string.Format("http://{0}:{1}/{2}", hostIp, RabbitApiPort, RabbitNodesApiPath);
                var request = CreateRequest(requestUrl, user, password);
                var rabbitNodes = restClient.Execute<List<Node>>(request);
                if (rabbitNodes.Data.Count != expectedNodeCount)
                {
                    var host = connectionString.Split(new[] {';'}).First();
                    Logger.Error("Node count in {0} is incorrect. Expecting {1} but was {2}", host, expectedNodeCount,
                        rabbitNodes.Data.Count);
                }
            }
            catch (Exception ex)
            {
                var host = connectionString.Split(new[] {';'}).First();
                LogError(host, ex);
            }
        }

        private static string GetUserFrom(string connectionString)
        {
            return GetDataFrom(connectionString, "username");
        }

        private static string GetPasswordFrom(string connectionString)
        {
            return GetDataFrom(connectionString, "password");
        }

        private static string GetDataFrom(string connectionString, string token)
        {
            return connectionString.Split(new[] { ';' }).First(s => s.Contains(token)).Split(new[] { '=' }).Last();
        }

        private static RestClient GetRestClient()
        {
            var restClient = new RestClient();
            restClient.AddHandler("application/json", new JsonDeserializer());
            return restClient;
        }


        public static T ParseJsonObject<T>(string json) where T : class, new()
        {
            JObject jobject = JObject.Parse(json);
            return JsonConvert.DeserializeObject<T>(jobject.ToString());
        }

        public virtual IRestRequest CreateRequest(string url, string username, string password)
        {
            IRestRequest request = new RestRequest(url, Method.GET) {RequestFormat = DataFormat.Json};

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            AddAuthorizationHeader(request, username, password);

            return request;
        }

        private static void AddAuthorizationHeader(IRestRequest request, string username, string password)
        {
            var credentials = String.Format("{0}:{1}", username, password);
            var bytes = Encoding.ASCII.GetBytes(credentials);
            var base64 = Convert.ToBase64String(bytes);
            var authorization = String.Concat("Basic ", base64);
            request.AddHeader("Authorization", authorization);
        }

        private static void LogError(string host, Exception exception)
        {
            var message = string.Format("Error while connecting to Rabbit queue on {0}", host);
            Logger.Error(message, exception);
        }
    }

    internal class MonitorMessage
    {
        public string Text
        {
            get { return "Monitor test message"; }
        }
    }

    internal class ExchangeType
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
    }

    internal class AuthMechanism
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
    }

    internal class Application
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
    }

    internal class Context
    {
        public string Description { get; set; }
        public string Path { get; set; }
        public int Port { get; set; }
    }

    internal class Node
    {
        public List<object> Partitions { get; set; }
// ReSharper disable once InconsistentNaming
        public string Os_pid { get; set; }
// ReSharper disable once InconsistentNaming
        public int Fd_used { get; set; }
// ReSharper disable once InconsistentNaming
        public int Fd_total { get; set; }
// ReSharper disable once InconsistentNaming
        public int Sockets_used { get; set; }
// ReSharper disable once InconsistentNaming
        public int Sockets_total { get; set; }
// ReSharper disable once InconsistentNaming
        public int Mem_used { get; set; }
// ReSharper disable once InconsistentNaming
        public object Mem_limit { get; set; }
// ReSharper disable once InconsistentNaming
        public bool Mem_alarm { get; set; }
// ReSharper disable once InconsistentNaming
        public int Disk_free_limit { get; set; }
// ReSharper disable once InconsistentNaming
        public object Disk_free { get; set; }
// ReSharper disable once InconsistentNaming
        public bool Disk_free_alarm { get; set; }
// ReSharper disable once InconsistentNaming
        public int Proc_used { get; set; }
// ReSharper disable once InconsistentNaming
        public int Proc_total { get; set; }
// ReSharper disable once InconsistentNaming
        public string Statistics_level { get; set; }
        public int Uptime { get; set; }
// ReSharper disable once InconsistentNaming
        public int Run_queue { get; set; }
        public int Processors { get; set; }
// ReSharper disable once InconsistentNaming
        public List<ExchangeType> Exchange_types { get; set; }
// ReSharper disable once InconsistentNaming
        public List<AuthMechanism> Auth_mechanisms { get; set; }
        public List<Application> Applications { get; set; }
        public List<Context> Contexts { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Running { get; set; }
    }
}