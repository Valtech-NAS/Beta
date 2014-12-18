namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using System;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;

    public interface IVacancyDataService
    {
        /// <summary>
        /// returns vacancy details
        /// </summary>
        /// <param name="vacancyId">id for the vacancy to retrieve</param>
        /// <returns>vacancy detail or null</returns>
        ApprenticeshipVacancyDetail GetVacancyDetails(int vacancyId);
    }
}
