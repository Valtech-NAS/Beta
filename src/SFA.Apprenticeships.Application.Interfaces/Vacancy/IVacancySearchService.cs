namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using System.Collections.Generic;
    using Domain.Entities.Vacancy;
    using Domain.Entities.Location;

    //TODO: Flush out API. need to understand valid combinations of location name, postcode, distance, keyword, etc. (include national)
    public interface IVacancySearchService
    {
        /// <summary>
        /// returns vacancies matching search criteria
        /// </summary>
        /// <param name="location">location previously obtained from postcode or placename search</param>
        /// <param name="searchRadius">in miles</param>
        /// <returns>0..* matching vacancies</returns>
        IEnumerable<VacancySummary> Search(Location location, int searchRadius);

    }
}
