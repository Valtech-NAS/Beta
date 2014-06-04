namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using System.Collections.Generic;
    using Domain.Entities.Vacancy;

    public interface IVacancyService //todo: rename IVacancyService to be more descriptive
    {
        int GetVacancyPageCount(VacancyLocationType vacancyLocationType);

        IEnumerable<VacancySummary> GetVacancySummary(VacancyLocationType vacancyLocationType, int page);
    }
}
