namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using Domain.Entities.Vacancies;
    using Search;

    public interface IVacancySearchService<TVacancySummaryResponse, out TVacancyDetail> 
        where TVacancySummaryResponse : VacancySummary
        where TVacancyDetail : VacancyDetail
    {
        /// <summary>
        /// returns vacancies matching search criteria
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>0..* matching vacancies</returns>
        SearchResults<TVacancySummaryResponse> Search(SearchParameters parameters);

        TVacancyDetail GetVacancyDetails(int vacancyId);
    }
}