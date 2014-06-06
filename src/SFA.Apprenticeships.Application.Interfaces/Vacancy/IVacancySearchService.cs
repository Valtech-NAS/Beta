namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using Domain.Entities.Vacancy;
    using Domain.Entities.Location;
    using SFA.Apprenticeships.Application.Interfaces.Search;

    //TODO: Flush out API. need to understand valid combinations of location name, postcode, distance, keyword, etc. (include national)
    public interface IVacancySearchService
    {
        /// <summary>
        /// returns vacancies matching search criteria
        /// </summary>
        /// <param name="location">location previously obtained from postcode or placename search</param>
        /// <param name="searchRadius">in miles</param>
        /// <returns>0..* matching vacancies</returns>
        SearchResults<VacancySummary> Search(Location location, int searchRadius);

    }
}
