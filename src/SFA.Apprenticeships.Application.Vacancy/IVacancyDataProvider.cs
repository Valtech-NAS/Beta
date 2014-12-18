namespace SFA.Apprenticeships.Application.Vacancy
{
    using SFA.Apprenticeships.Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;

    public interface IVacancyDataProvider
    {
        ApprenticeshipVacancyDetail GetVacancyDetails(int vacancyId);
    }
}