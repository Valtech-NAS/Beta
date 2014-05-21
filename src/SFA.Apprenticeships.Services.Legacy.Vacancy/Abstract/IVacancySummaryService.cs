namespace SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Common.Entities.Vacancy;
    using SFA.Apprenticeships.Common.Interfaces.Enums;

    public interface IVacancySummaryService
    {
        int GetVacancyCount(VacancyLocationType vacancyLocationType);

        IEnumerable<VacancySummary> GetVacancySummary(VacancyLocationType vacancyLocationType, int page);
    }
}
