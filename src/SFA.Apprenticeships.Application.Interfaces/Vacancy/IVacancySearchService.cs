namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using System.Collections.Generic;
    using Domain.Entities.Vacancy;

    public interface IVacancySearchService
    {
        //TODO: Flush out API. need to understand valid combinations of location name, postcode, distance, keyword, etc.
        IEnumerable<VacancySummary> Search(string postcodeOrLocationName, int searchRadius);
    }
}
