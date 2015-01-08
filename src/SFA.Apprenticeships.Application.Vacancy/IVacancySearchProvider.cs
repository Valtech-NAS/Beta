namespace SFA.Apprenticeships.Application.Vacancy
{
    using Interfaces.Search;

    public interface IVacancySearchProvider<TVacancySummaryResponse, TSearchParameters> 
        where TVacancySummaryResponse : class
        where TSearchParameters : SearchParametersBase
    {
        SearchResults<TVacancySummaryResponse> FindVacancies(TSearchParameters parameters);
    }
}