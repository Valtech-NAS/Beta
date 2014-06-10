namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.Services
{
    using System;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;

    public interface IVacancyIndexerService
    {
        void Index(VacancySummaryUpdate vacancySummaryToIndex);

        /// <summary>
        /// Returns the number of items in the index that are not indexed 
        /// with the <param name="updateReference"></param> supplied.
        /// </summary>
        /// <param name="updateReference"></param>
        /// <returns></returns>
        int VacanciesWithoutUpdateReference(Guid updateReference);

        /// <summary>
        /// Deletes all vacancies where the <param name="updateReference"></param> does
        /// not match.
        /// </summary>
        /// <param name="updateReference"></param>
        /// <returns></returns>
        void ClearObsoleteVacancie(Guid updateReference);
    }
}
