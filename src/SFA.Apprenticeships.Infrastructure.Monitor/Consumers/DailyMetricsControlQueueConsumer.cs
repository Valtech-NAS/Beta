﻿namespace SFA.Apprenticeships.Infrastructure.Monitor.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Azure.Common.Messaging;
    using Tasks;

    public class DailyMetricsControlQueueConsumer : AzureControlQueueConsumer
    {
        private const string QueueName = "dailymetricsscheduler";

        private readonly IDailyMetricsTasksRunner _dailyMetricsTasksRunner;
        private readonly IConfigurationManager _configurationManager;

        public DailyMetricsControlQueueConsumer(
            IProcessControlQueue<StorageQueueMessage> messageService,
            IDailyMetricsTasksRunner dailyMetricsTasksRunner,
            IConfigurationManager configurationManager,
            ILogService logger)
            : base(messageService, logger, "DailyMetrics", QueueName)
        {
            _dailyMetricsTasksRunner = dailyMetricsTasksRunner;
            _configurationManager = configurationManager;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var monitorScheduleMessage = GetLatestQueueMessage();

                if (monitorScheduleMessage != null)
                {
                    if (IsDailyMetricsEnabled())
                    {
                        _dailyMetricsTasksRunner.RunDailyMetricsTasks();
                    }

                    MessageService.DeleteMessage(monitorScheduleMessage.MessageId, monitorScheduleMessage.PopReceipt, QueueName);
                }
            });
        }

        private bool IsDailyMetricsEnabled()
        {   
            return _configurationManager.GetCloudAppSetting<bool>("DailyMetricsEnabled");
        }
    }
}