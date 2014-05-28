namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;

    public interface IVacancySearchService
    {
        //TODO: Flush out API
        IEnumerable<VacancySummary> Search();
    }
}
