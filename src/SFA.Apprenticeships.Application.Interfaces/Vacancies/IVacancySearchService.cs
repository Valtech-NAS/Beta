namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using Domain.Entities.Vacancies;
    using Search;

    public interface IVacancySearchService<TVacancySummaryResponse> where TVacancySummaryResponse : VacancySummary
    {
        /// <summary>
        /// returns vacancies matching search criteria
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>0..* matching vacancies</returns>
        SearchResults<TVacancySummaryResponse> Search(SearchParameters parameters);
    }
}