namespace SFA.Apprenticeships.Services.VacancyEtl.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using EasyNetQ;
    using SFA.Apprenticeships.Common.Interfaces.Enums;
    using SFA.Apprenticeships.Common.Messaging.Interfaces;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract;
    using SFA.Apprenticeships.Services.VacancyEtl.Entities;

    public class VacancySchedulerConsumer
    {
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
                // Check Rabbit procesing queue - should not be doing any still or there is a potential issue.
                // TODO: Log it.

                var nationalCount = _vacancySummaryService.GetVacancyPageCount(VacancyLocationType.National);
                var nonNationalCount = _vacancySummaryService.GetVacancyPageCount(VacancyLocationType.NonNational);
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

            var dcs = new XmlSerializer(typeof(StorageQueueMessage));

            using (var xmlstream = new MemoryStream(Encoding.Unicode.GetBytes(queueMessage.AsString)))
            {
                scheduledQueueMessage = (StorageQueueMessage)dcs.Deserialize(xmlstream);
            }

            _cloudClient.DeleteMessage(VacancySearchDataControlQueueName, queueMessage);
            return scheduledQueueMessage;
        }

        private IEnumerable<VacancySummaryPage> BuildVacancySummaries(Guid updateReferenceId, int nationalCount, int nonNationalCount)
        {
            var totalCount = nationalCount + nonNationalCount;
            var vacancySumaries = new List<VacancySummaryPage>(totalCount);

            for (int i = 0; i < nationalCount; i++)
            {
                var vacancySummaryPage = new VacancySummaryPage
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
                var vacancySummaryPage = new VacancySummaryPage
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
