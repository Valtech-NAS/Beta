namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using System.Collections.Generic;
    using Domain.Entities.Vacancy;

    public interface IVacancyService
    {
        int GetVacancyPageCount(VacancyLocationType vacancyLocationType);

        IEnumerable<VacancySummary> GetVacancySummary(VacancyLocationType vacancyLocationType, int page);
    }
}
