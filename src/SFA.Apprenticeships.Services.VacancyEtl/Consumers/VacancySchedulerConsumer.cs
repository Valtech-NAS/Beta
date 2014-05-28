﻿namespace SFA.Apprenticeships.Services.VacancyEtl.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using EasyNetQ;
    using Microsoft.WindowsAzure.Storage.Queue;
    using SFA.Apprenticeships.Domain.Interfaces.Enums;
    using SFA.Apprenticeships.Services.VacancyEtl.Entities;

    public class VacancySchedulerConsumer
    {
        private readonly static NLog.Logger Logger = NLog.LogManager.GetLogger(Constants.NamedLoggers.VacanyImporterLogger);

        private readonly IBus _bus;
        private readonly IAzureCloudClient _cloudClient;
        private readonly IVacancySummaryService _vacancySummaryService;
        private const string VacancySearchDataControlQueueName = "vacancysearchdatacontrol";

        public VacancySchedulerConsumer(IBus bus, IAzureCloudClient cloudClient, IVacancySummaryService vacancySummaryService)
        {
            _bus = bus;
            _cloudClient = cloudClient;
            _vacancySummaryService = vacancySummaryService;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() => CheckQueue());
        }

        private void CheckQueue()
        {
            var latestScheduledMessage = GetLatestQueueMessage();

            if (latestScheduledMessage != null)
            {
                // TODO: Check Rabbit procesing queue - should not still be processing messages or there maybe a potential issue.

                Logger.Info("Scheduled VacancyEtl Message Received at: {0}", DateTime.Now);
                var scheduledMessageData = DeserialiseQueueMessage(latestScheduledMessage);

                var nationalCount = _vacancySummaryService.GetVacancyPageCount(VacancyLocationType.National);
                var nonNationalCount = _vacancySummaryService.GetVacancyPageCount(VacancyLocationType.NonNational);
                var vacancySumaries = BuildVacancySummaries(Guid.Parse(scheduledMessageData.ClientRequestId), nationalCount, nonNationalCount);

                // Only delete from queue once we have all vacanies from the services without error.
                _cloudClient.DeleteMessage(VacancySearchDataControlQueueName, latestScheduledMessage);

                Parallel.ForEach(
                    vacancySumaries,
                    new ParallelOptions {MaxDegreeOfParallelism = 10},
                    vacancy => _bus.Publish(vacancy));
            }
        }

        private CloudQueueMessage GetLatestQueueMessage()
        {
            var queueMessage = _cloudClient.GetMessage(VacancySearchDataControlQueueName);

            if (queueMessage == null)
            {
                return null;
            }

            while (true)
            {
                var nextQueueMessage = _cloudClient.GetMessage(VacancySearchDataControlQueueName);
                if (nextQueueMessage == null)
                {
                    // We have the latest message on the queue.
                    break;
                }

                _cloudClient.DeleteMessage(VacancySearchDataControlQueueName, queueMessage);
                queueMessage = nextQueueMessage;
            }

            return queueMessage;
        }

        private StorageQueueMessage DeserialiseQueueMessage(CloudQueueMessage queueMessage)
        {
            StorageQueueMessage scheduledQueueMessage;

            var dcs = new XmlSerializer(typeof(StorageQueueMessage));

            using (var xmlstream = new MemoryStream(Encoding.Unicode.GetBytes(queueMessage.AsString)))
            {
                scheduledQueueMessage = (StorageQueueMessage)dcs.Deserialize(xmlstream);
            }

            return scheduledQueueMessage;
        }

        private IEnumerable<VacancySummaryPage> BuildVacancySummaries(Guid updateReferenceId, int nationalCount, int nonNationalCount)
        {
            var totalCount = nationalCount + nonNationalCount;
            var vacancySumaries = new List<VacancySummaryPage>(totalCount);

            for (int i = 1; i <= nationalCount; i++)
            {
                var vacancySummaryPage = new VacancySummaryPage
                {
                    PageNumber = i,
                    TotalPages = totalCount,
                    UpdateReference = updateReferenceId,
                    VacancyLocation = VacancyLocationType.National
                };

                vacancySumaries.Add(vacancySummaryPage);
            }

            for (int i = nationalCount + 1; i <= totalCount; i++)
            {
                var vacancySummaryPage = new VacancySummaryPage
                {
                    PageNumber = i,
                    TotalPages = totalCount,
                    UpdateReference = updateReferenceId,
                    VacancyLocation = VacancyLocationType.NonNational
                };

                vacancySumaries.Add(vacancySummaryPage);
            }

            return vacancySumaries;
        }
    }
}
