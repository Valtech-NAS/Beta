namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using Domain.Entities.Location;
    using Search;

    //TODO: Flush out API. need to understand valid combinations of location name, postcode, distance, keyword, etc. (include national)
    public interface IVacancySearchService
    {
        /// <summary>
        /// returns vacancies matching search criteria
        /// </summary>
        /// <param name="location">location previously obtained from postcode or placename search</param>
        /// <param name="searchRadius">in miles</param>
        /// <returns>0..* matching vacancies</returns>
        SearchResults<VacancySummaryResponse> Search(string jobTitle, string keywords, Location location, int pageNumber, int pageSize, int searchRadius);
    }
}
