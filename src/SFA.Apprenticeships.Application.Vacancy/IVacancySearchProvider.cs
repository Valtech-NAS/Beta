namespace SFA.Apprenticeships.Application.Vacancy
{
    using Interfaces.Search;

    public interface IVacancySearchProvider<TVacancySummaryResponse, TSearchParameters> 
        where TVacancySummaryResponse : class
        where TSearchParameters : SearchParametersBase
    {
        SearchResults<TVacancySummaryResponse, TSearchParameters> FindVacancies(TSearchParameters parameters);

        SearchResults<TVacancySummaryResponse, TSearchParameters> FindVacancy(string vacancyReference);
    }
}