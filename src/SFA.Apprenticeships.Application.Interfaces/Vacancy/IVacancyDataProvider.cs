namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using Domain.Entities.Vacancy;

    public interface IVacancyDataProvider
    {
        VacancyDetail GetVacancyDetails(int vacancyId);
    }
}
