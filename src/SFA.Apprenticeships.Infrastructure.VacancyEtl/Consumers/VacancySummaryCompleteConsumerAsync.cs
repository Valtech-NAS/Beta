namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using Microsoft.WindowsAzure;
    using NLog;
    using SFA.Apprenticeships.Infrastructure.PerformanceCounters;
    using VacancyIndexer;

    public class VacancySummaryCompleteConsumerAsync : IConsumeAsync<VacancySummaryUpdateComplete>
    {
        private const string VacancyEtlPerformanceCounterCategory = "SFA.Apprenticeships.WorkerRoles.VacancyEtl";
        private const string VacancyIndexCounter = "VacancyEtlExecutions";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyIndexerService _vacancyIndexer;
        private readonly IPerformanceCounterService _performanceCounterService;

        public VacancySummaryCompleteConsumerAsync(IVacancyIndexerService vacancyIndexer, 
            IPerformanceCounterService performanceCounterService)
        {
            _vacancyIndexer = vacancyIndexer;
            _performanceCounterService = performanceCounterService;
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryCompleteConsumerAsync")]
        public Task Consume(VacancySummaryUpdateComplete updateComplete)
        {
            Logger.Debug("Received vacancy summary update completed message.");

            return Task.Run(() =>
            {
                if (IndexIsCorrectlyCreated(updateComplete.ScheduledRefreshDateTime))
                {
                    Logger.Debug("Swapping index alias after vacancy summary update completed");
                    _vacancyIndexer.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                    Logger.Debug("Index swapped after vacancy summary update completed");

                    IncrementVacancyIndexPerformanceCounter();
                }
                else
                {
                    Logger.Error("The new index is not correctly created. Aborting swap.");
                }
            });
        }

        private bool IndexIsCorrectlyCreated(DateTime scheduledRefreshDateTime)
        {
            return _vacancyIndexer.IsIndexCorrectlyCreated(scheduledRefreshDateTime);
        }

        private void IncrementVacancyIndexPerformanceCounter()
        {
            bool performanceCountersEnabled;

            if (bool.TryParse(CloudConfigurationManager.GetSetting("PerformanceCountersEnabled"), out performanceCountersEnabled)
                && performanceCountersEnabled)
            {
                _performanceCounterService.IncrementCounter(VacancyEtlPerformanceCounterCategory, VacancyIndexCounter);
            }
        }
    }
}
