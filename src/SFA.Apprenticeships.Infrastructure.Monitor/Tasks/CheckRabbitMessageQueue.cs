namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Linq;
    using Domain.Interfaces.Configuration;
    using EasyNetQ;
    using NLog;

    public class CheckRabbitMessageQueue : IMonitorTask
    {
        private const string MonitorRabbitAppSetting = "Monitor.Rabbit";
        private const string MonitorRelkAppSetting = "Monitor.Relk";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _rabbitConnectionString;
        private readonly string _relkConnectionString;
        private IBus _rabbitBus;

        public CheckRabbitMessageQueue(IConfigurationManager configurationManager)
        {
            _rabbitConnectionString = configurationManager.GetAppSetting(MonitorRabbitAppSetting);
            _relkConnectionString = configurationManager.GetAppSetting(MonitorRelkAppSetting);
        }

        public string TaskName
        {
            get { return "Check rabbit message queue"; }
        }

        public void Run()
        {
            CheckRabbitQueue(_rabbitConnectionString);
            CheckRabbitQueue(_relkConnectionString);
        }

        private void CheckRabbitQueue(string connectionString)
        {
            _rabbitBus = RabbitHutch.CreateBus(connectionString);

            try
            {
                _rabbitBus.Publish(new MonitorMessage());
            }
            catch (Exception ex)
            {
                var host = connectionString.Split(new [] {';'}).First();
                LogError(host, ex);
            }
            finally
            {
                 _rabbitBus.Dispose();
            }
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
}
