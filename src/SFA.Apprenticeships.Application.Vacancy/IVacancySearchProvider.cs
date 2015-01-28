namespace SFA.Apprenticeships.Application.Vacancy
{
    using Interfaces.Search;

    public interface IVacancySearchProvider<TVacancySummaryResponse, in TSearchParameters> 
        where TVacancySummaryResponse : class
        where TSearchParameters : SearchParametersBase
    {
        SearchResults<TVacancySummaryResponse> FindVacancies(TSearchParameters parameters);

        SearchResults<TVacancySummaryResponse> FindVacancy(string vacancyReference);
    }
}