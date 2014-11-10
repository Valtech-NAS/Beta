namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Configuration;
    using EasyNetQ;
    using NLog;

    public class CheckRabbitMessageQueue : IMonitorTask
    {
        private const string MonitorAppSetting = "Monitor.Rabbit";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _connectionString;
        private IBus _rabbitBus;

        public CheckRabbitMessageQueue(IConfigurationManager configurationManager)
        {
            _connectionString = configurationManager.GetAppSetting(MonitorAppSetting);
        }

        public string TaskName
        {
            get { return "Check rabbit message queue"; }
        }

        public void Run()
        {
            _rabbitBus = RabbitHutch.CreateBus(_connectionString);

            try
            {
                _rabbitBus.Publish(new MonitorMessage() );
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
                _rabbitBus.Dispose();
            }
        }

        private static void LogError(Exception exception)
        {
            Logger.Error("Error while connecting to Rabbit queue", exception);
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
