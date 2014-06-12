namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.Services
{
    using System;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;

    public interface IVacancyIndexerService
    {
        /// <summary>
        /// Indexes the vacancy summary to an elasticsearch index.
        /// The index the summary is indexed to should not be referenced by anything.
        /// See <see cref="SwapIndex"/> for more details.
        /// </summary>
        /// <param name="vacancySummaryToIndex"></param>
        void Index(VacancySummaryUpdate vacancySummaryToIndex);

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
    }
}
