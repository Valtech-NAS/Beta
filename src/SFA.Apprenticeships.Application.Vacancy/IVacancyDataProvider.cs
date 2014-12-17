namespace SFA.Apprenticeships.Application.Vacancy
{
    using SFA.Apprenticeships.Domain.Entities.Vacancies;

    public interface IVacancyDataProvider
    {
        VacancyDetail GetVacancyDetails(int vacancyId);
    }
}