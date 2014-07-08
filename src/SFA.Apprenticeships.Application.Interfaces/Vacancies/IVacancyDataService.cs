namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using System;
    using Domain.Entities.Vacancies;

    public interface IVacancyDataService
    {
        /// <summary>
        /// returns vacancy details
        /// </summary>
        /// <param name="vacancyId">id for the vacancy to retrieve</param>
        /// <returns>vacancy detail or null</returns>
        VacancyDetail GetVacancyDetails(int vacancyId);
    }
}
