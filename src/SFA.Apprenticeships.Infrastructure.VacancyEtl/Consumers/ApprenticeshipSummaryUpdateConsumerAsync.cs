﻿namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces.ReferenceData;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using EasyNetQ.AutoSubscribe;
    using Elastic.Common.Entities;
    using NLog;
    using VacancyIndexer;

    public class ApprenticeshipSummaryUpdateConsumerAsync : IConsumeAsync<ApprenticeshipSummaryUpdate>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IReferenceDataService _referenceDataService;
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> _vacancyIndexer;
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;

        public ApprenticeshipSummaryUpdateConsumerAsync(
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> vacancyIndexer,
            IVacancySummaryProcessor vacancySummaryProcessor, IReferenceDataService referenceDataService)
        {
            _vacancyIndexer = vacancyIndexer;
            _vacancySummaryProcessor = vacancySummaryProcessor;
            _referenceDataService = referenceDataService;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "ApprenticeshipSummaryUpdateConsumerAsync")]
        public Task Consume(ApprenticeshipSummaryUpdate vacancySummaryToIndex)
        {
            return Task.Run(() =>
            {
                try
                {
                    PopulateCategoriesCodes(vacancySummaryToIndex);

                    _vacancyIndexer.Index(vacancySummaryToIndex);
                    _vacancySummaryProcessor.QueueVacancyIfExpiring(vacancySummaryToIndex);
                }
                catch (Exception ex)
                {
                    var message = string.Format("Failed indexing vacancy summary {0}", vacancySummaryToIndex.Id);
                    Logger.Warn(message, ex);
                }
            });
        }

        private void PopulateCategoriesCodes(ApprenticeshipSummaryUpdate vacancySummaryToIndex)
        {
            var categories = _referenceDataService.GetCategories().ToArray();

            vacancySummaryToIndex.SectorCode = categories
                .First(c => c.FullName == vacancySummaryToIndex.Sector).CodeName;

            vacancySummaryToIndex.FrameworkCode = categories
                .First(c => c.FullName == vacancySummaryToIndex.Sector)
                .SubCategories.First(sc => sc.FullName == vacancySummaryToIndex.Framework)
                .CodeName;
        }
    }
}