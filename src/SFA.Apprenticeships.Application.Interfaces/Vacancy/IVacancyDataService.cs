namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using Domain.Entities.Vacancy;

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
