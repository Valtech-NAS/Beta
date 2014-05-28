namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;

    public interface IVacancySummaryService
    {
        int GetVacancyPageCount(VacancyLocationType vacancyLocationType);

        IEnumerable<VacancySummary> GetVacancySummary(VacancyLocationType vacancyLocationType, int page);
    }
}
