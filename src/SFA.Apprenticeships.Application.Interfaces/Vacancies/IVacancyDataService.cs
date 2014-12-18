namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using Domain.Entities.Vacancies;

    public interface IVacancyDataService<out TVacancyDetail> where TVacancyDetail : VacancyDetail
    {
        /// <summary>
        /// returns vacancy details
        /// </summary>
        /// <param name="vacancyId">id for the vacancy to retrieve</param>
        /// <returns>vacancy detail or null</returns>
        TVacancyDetail GetVacancyDetails(int vacancyId);
    }
}
