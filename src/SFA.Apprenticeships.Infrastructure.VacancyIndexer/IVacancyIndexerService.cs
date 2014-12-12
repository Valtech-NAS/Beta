namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer
{
    using System;
    using Application.VacancyEtl.Entities;
    using Domain.Entities.Vacancies;
    using Elastic.Common.Entities;

    public interface IVacancyIndexerService<in TSourceSummary, in TDestinationSummary>
        where TSourceSummary : VacancySummary, IVacancyUpdate
        where TDestinationSummary : class, IVacancySummary
    {
        /// <summary>
        /// Indexes the vacancy summary to an elasticsearch index.
        /// The index the summary is indexed to should not be referenced by anything.
        /// See <see cref="SwapIndex"/> for more details.
        /// </summary>
        /// <param name="vacancySummaryToIndex"></param>
        void Index(TSourceSummary vacancySummaryToIndex);

        /// <summary>
        /// Creates an index to be used for the latest update 
        /// of vacancy summary information.
        /// </summary>
        /// <param name="scheduledRefreshDateTime"></param>
        void CreateScheduledIndex(DateTime scheduledRefreshDateTime);

        /// <summary>
        /// Swaps any existing aliases used for serving vacacny summary data
        /// to use the new index that contains the latest indexed data.
        /// </summary>
        /// <param name="scheduledRefreshDateTime"></param>
        void SwapIndex(DateTime scheduledRefreshDateTime);

        /// <summary>
        /// Checks if the index is correctly created
        /// </summary>
        /// <returns>True if the index is correctly created. Otherwise false.</returns>
        bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime);
    }
}
