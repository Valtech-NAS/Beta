namespace SFA.Apprenticeships.Application.Vacancy
{
    using Interfaces.Search;
    
    public interface IVacancySearchProvider<TVacancySummaryResponse> where TVacancySummaryResponse : class
    {
        SearchResults<TVacancySummaryResponse> FindVacancies(SearchParameters parameters);
    }
}