namespace SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using EasyNetQ;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using SFA.Apprenticeships.Common.Interfaces.Enums;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract;
    using SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Entities;

    public class VacancySchedulerConsumer
    {
        private readonly IBus _bus;
        private readonly IVacancySummaryService _vacancySummaryService;
        private readonly CloudQueueClient _queueClient;
        private readonly string _queueName;

        public VacancySchedulerConsumer(IBus bus, IVacancySummaryService vacancySummaryService)
        {
            _bus = bus;
            _vacancySummaryService = vacancySummaryService;

            var queueConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            var storageAccount = CloudStorageAccount.Parse(queueConnectionString);

            // Create the queue client
            _queueClient = storageAccount.CreateCloudQueueClient();
            _queueName = CloudConfigurationManager.GetSetting("QueueName");
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
                // Check Rabbit procesing queue - should not be doing any still or there is a potential issue.
                // TODO: Log it.

                var nationalCount = _vacancySummaryService.GetVacancyCount(VacancyLocationType.National);
                var nonNationalCount = _vacancySummaryService.GetVacancyCount(VacancyLocationType.NonNational);
                var vacancySumaries = BuildVacancySummaries(Guid.Parse(latestScheduledMessage.ClientRequestId), nationalCount, nonNationalCount);

                Parallel.ForEach(
                    vacancySumaries,
                    new ParallelOptions {MaxDegreeOfParallelism = 10},
                    vacancy => _bus.Publish(vacancy));
            }
        }

        private StorageQueueMessage GetLatestQueueMessage()
        {
            StorageQueueMessage scheduledQueueMessage;
            var queue = _queueClient.GetQueueReference(_queueName);
            var queueMessage = queue.GetMessage();

            if (queueMessage == null)
            {
                return null;
            }

            while (true)
            {
                var nextQueueMessage = queue.GetMessage();
                if (nextQueueMessage == null)
                {
                    // We have the latest message on the queue.
                    break;
                }

                queue.DeleteMessage(queueMessage);
                queueMessage = nextQueueMessage;
            }
            
            var dcs = new XmlSerializer(typeof(StorageQueueMessage));

            using (var xmlstream = new MemoryStream(Encoding.Unicode.GetBytes(queueMessage.AsString)))
            {
                scheduledQueueMessage = (StorageQueueMessage)dcs.Deserialize(xmlstream);
            }
            
            queue.DeleteMessage(queueMessage);
            return scheduledQueueMessage;
        }

        private IEnumerable<VacancySummaryPage> BuildVacancySummaries(Guid updateReferenceId, int nationalCount, int nonNationalCount)
        {
            var totalCount = nationalCount + nonNationalCount;
            var vacancySumaries = new List<VacancySummaryPage>(totalCount);

            for (int i = 0; i < nationalCount; i++)
            {
                var vacancySummaryPage = new VacancySummaryPage()
                {
                    PageNumber = i + 1,
                    TotalPages = totalCount,
                    UpdateReference = updateReferenceId,
                    VacancyLocation = VacancyLocationType.National
                };

                vacancySumaries.Add(vacancySummaryPage);
            }

            for (int i = nationalCount; i < totalCount; i++)
            {
                var vacancySummaryPage = new VacancySummaryPage()
                {
                    PageNumber = nationalCount + 1,
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
