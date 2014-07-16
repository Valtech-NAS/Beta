namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using System;
    using Domain.Entities.Locations;
    using Search;

    // TODO: NOTIMPL: Flush out API. need to understand valid combinations of location name, postcode, distance, keyword, etc. (include national)
    public interface IVacancySearchService
    {
        /// <summary>
        /// returns vacancies matching search criteria
        /// </summary>
        /// <param name="keywords">keywords to be searched</param>
        /// <param name="location">location previously obtained from postcode or placename search</param>
        /// <param name="pageNumber">current page</param>
        /// <param name="pageSize">number of results per page</param>
        /// <param name="searchRadius">in miles</param>
        /// <param name="sortType">the sort order for the results</param>
        /// <returns>0..* matching vacancies</returns>
        SearchResults<VacancySummaryResponse> Search(string keywords,
            Location location,
            int pageNumber,
            int pageSize,
            int searchRadius,
            VacancySortType sortType);
    }
}
