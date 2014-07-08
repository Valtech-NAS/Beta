namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using System;
    using Domain.Entities.Vacancies;

    public interface IVacancyDataProvider
    {
        VacancyDetail GetVacancyDetails(int vacancyId);
    }
}
