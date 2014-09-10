namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Configuration;
    using EasyNetQ;
    using NLog;

    public class CheckRabbitMessageQueue : IMonitorTask
    {
        private const string MonitorAppSetting = "Monitor.Rabbit";
        private const string TaskName = "Check Rabbit MessageQueue";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _connectionString;
        private IBus _rabbitBus;

        public CheckRabbitMessageQueue(IConfigurationManager configurationManager)
        {
            _connectionString = configurationManager.GetAppSetting(MonitorAppSetting);
        }

        public void Run()
        {
            Logger.Debug(string.Format("Start running task {0}", TaskName));

            _rabbitBus = RabbitHutch.CreateBus(_connectionString);

            try
            {
                _rabbitBus.PublishAsync(new MonitorMessage()).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        LogError(task.Exception);
                    }
                });
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
                _rabbitBus.Dispose();
                Logger.Debug(string.Format("Finished running task {0}", TaskName));
            }
        }

        private void LogError(Exception exception)
        {
            Logger.ErrorException("Error while connecting to Rabbit queue", exception);
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