namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Application.Interfaces.ReferenceData;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using EasyNetQ.AutoSubscribe;
    using Elastic.Common.Entities;
    using VacancyIndexer;

    public class ApprenticeshipSummaryUpdateConsumerAsync : IConsumeAsync<ApprenticeshipSummaryUpdate>
    {
        private readonly IReferenceDataService _referenceDataService;
        private readonly ILogService _logService;
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> _vacancyIndexer;
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;

        public ApprenticeshipSummaryUpdateConsumerAsync(
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> vacancyIndexer,
            IVacancySummaryProcessor vacancySummaryProcessor, IReferenceDataService referenceDataService, ILogService logService)
        {
            _vacancyIndexer = vacancyIndexer;
            _vacancySummaryProcessor = vacancySummaryProcessor;
            _referenceDataService = referenceDataService;
            _logService = logService;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "ApprenticeshipSummaryUpdateConsumerAsync")]
        public Task Consume(ApprenticeshipSummaryUpdate vacancySummaryToIndex)
        {
            return Task.Run(() =>
            {
                if (PopulateCategoriesCodes(vacancySummaryToIndex))
                {
                    _vacancyIndexer.Index(vacancySummaryToIndex);
                    _vacancySummaryProcessor.QueueVacancyIfExpiring(vacancySummaryToIndex);
                }
            });
        }

        private bool PopulateCategoriesCodes(ApprenticeshipSummaryUpdate vacancySummaryToIndex)
        {
            var categories = _referenceDataService.GetCategories().ToArray();

            vacancySummaryToIndex.SectorCode = categories
                .First(c => c.FullName == vacancySummaryToIndex.Sector).CodeName;

            if (categories.First(c => c.FullName == vacancySummaryToIndex.Sector)
                .SubCategories.Any(sc => sc.FullName == vacancySummaryToIndex.Framework)
                )
            {
                vacancySummaryToIndex.FrameworkCode = categories
                    .First(c => c.FullName == vacancySummaryToIndex.Sector)
                    .SubCategories.First(sc => sc.FullName == vacancySummaryToIndex.Framework)
                    .CodeName;
                return true;
            }

            _logService.Warn("The vacancy with Id {0} has a mismatched Sector/Framework:{1} | {2}",
                vacancySummaryToIndex.Id, vacancySummaryToIndex.Sector, vacancySummaryToIndex.Framework);
            return false;

        }
    }
}
