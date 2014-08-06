namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using Domain.Entities.Vacancies;

    public interface IVacancyDataProvider
    {
        VacancyDetail GetVacancyDetails(int vacancyId);
    }
}
