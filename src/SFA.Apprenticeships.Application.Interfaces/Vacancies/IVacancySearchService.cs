namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using Search;

    public interface IVacancySearchService
    {
        /// <summary>
        /// returns vacancies matching search criteria
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>0..* matching vacancies</returns>
        SearchResults<VacancySummaryResponse> Search(SearchParameters parameters);
    }
}