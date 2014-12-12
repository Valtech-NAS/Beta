namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using Elastic.Common.Entities;
    using Microsoft.WindowsAzure;
    using NLog;
    using PerformanceCounters;
    using VacancyIndexer;

    public class VacancySummaryCompleteConsumerAsync : IConsumeAsync<VacancySummaryUpdateComplete>
    {
        private const string VacancyEtlPerformanceCounterCategory = "SFA.Apprenticeships.WorkerRoles.VacancyEtl";
        private const string VacancyIndexCounter = "VacancyEtlExecutions";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> _apprenticeshipIndexer;
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> _trainseeshipIndexer;
        private readonly IPerformanceCounterService _performanceCounterService;

        public VacancySummaryCompleteConsumerAsync(IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> apprenticeshipIndexer, 
            IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> trainseeshipIndexer,
            IPerformanceCounterService performanceCounterService)
        {
            _apprenticeshipIndexer = apprenticeshipIndexer;
            _trainseeshipIndexer = trainseeshipIndexer;
            _performanceCounterService = performanceCounterService;
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryCompleteConsumerAsync")]
        public Task Consume(VacancySummaryUpdateComplete updateComplete)
        {
            Logger.Debug("Received vacancy summary update completed message.");

            return Task.Run(() =>
            {
                if (_apprenticeshipIndexer.IsIndexCorrectlyCreated(updateComplete.ScheduledRefreshDateTime))
                {
                    Logger.Debug("Swapping apprenticeship index alias after vacancy summary update completed");
                    _apprenticeshipIndexer.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                    Logger.Debug("Index apprenticeship swapped after vacancy summary update completed");
                    IncrementVacancyIndexPerformanceCounter();
                }
                else
                {
                    Logger.Error("The new apprenticeship index is not correctly created. Aborting swap.");
                }

                if (_trainseeshipIndexer.IsIndexCorrectlyCreated(updateComplete.ScheduledRefreshDateTime))
                {
                    Logger.Debug("Swapping traineeship index alias after vacancy summary update completed");
                    _trainseeshipIndexer.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                    Logger.Debug("Index traineeship swapped after vacancy summary update completed");
                    IncrementVacancyIndexPerformanceCounter();
                }
                else
                {
                    Logger.Error("The new traineeship index is not correctly created. Aborting swap.");
                }
            });
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
