namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Vacancies;

    public interface IVacancyDataProvider<out TVacancyDetail> where TVacancyDetail : VacancyDetail
    {
        TVacancyDetail GetVacancyDetails(int vacancyId);
    }
}