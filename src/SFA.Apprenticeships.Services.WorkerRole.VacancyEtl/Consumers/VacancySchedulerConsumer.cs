namespace SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using EasyNetQ;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using SFA.Apprenticeships.Common.Interfaces.Enums;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract;

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
            var queue = _queueClient.GetQueueReference(_queueName);
            var message = queue.GetMessage();

            if (message != null)
            {
                // Check Rabbit procesing queue - should not be doing any still or there is a potential issue.
                // TODO: Log it.

                var nationalCount = _vacancySummaryService.GetVacancyCount(VacancyLocationType.National);
                var nonnationalCount = _vacancySummaryService.GetVacancyCount(VacancyLocationType.NonNational);

            }
        }
    }
}
