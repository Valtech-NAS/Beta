namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Microsoft.WindowsAzure;
    using NLog;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities;
    using SFA.Apprenticeships.Infrastructure.PerformanceCounters;
    using SFA.Apprenticeships.Infrastructure.VacancyIndexer;

    public class VacancySummaryCompleteConsumerAsync : IConsumeAsync<VacancySummaryUpdateComplete>
    {
        private const string VacancyEtlPerformanceCounterCategory = "SFA.Apprenticeships.WorkerRoles.VacancyEtl";
        private const string VacancyIndexCounter = "VacancyEtlExecutions";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>
            _apprenticeshipIndexer;

        private readonly IPerformanceCounterService _performanceCounterService;
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> _trainseeshipIndexer;
        private readonly IConfigurationManager _configurationManager;

        public VacancySummaryCompleteConsumerAsync(
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> apprenticeshipIndexer,
            IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> trainseeshipIndexer,
            IPerformanceCounterService performanceCounterService, 
            IConfigurationManager configurationManager)
        {
            _apprenticeshipIndexer = apprenticeshipIndexer;
            _trainseeshipIndexer = trainseeshipIndexer;
            _performanceCounterService = performanceCounterService;
            _configurationManager = configurationManager;
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
            if ( _configurationManager.GetCloudAppSetting<bool>("PerformanceCountersEnabled"))
            {
                _performanceCounterService.IncrementCounter(VacancyEtlPerformanceCounterCategory, VacancyIndexCounter);
            }
        }
    }
}