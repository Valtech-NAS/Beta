namespace SFA.Apprenticeships.Domain.Interfaces.Providers.Vacancy
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;

    public interface IVacancySummaryProvider
    {
        int GetVacancyPageCount(VacancyLocationType vacancyLocationType);

        IEnumerable<VacancySummary> GetVacancySummary(VacancyLocationType vacancyLocationType, int page);
    }
}
