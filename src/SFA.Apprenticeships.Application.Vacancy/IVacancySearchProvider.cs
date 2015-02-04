namespace SFA.Apprenticeships.Application.Vacancy
{
    using Interfaces.Search;
    using Interfaces.Vacancies;

    public interface IVacancySearchProvider<TVacancySummaryResponse, TSearchParameters> 
        where TVacancySummaryResponse : class
        where TSearchParameters : VacancySearchParametersBase
    {
        SearchResults<TVacancySummaryResponse, TSearchParameters> FindVacancies(TSearchParameters parameters);

        SearchResults<TVacancySummaryResponse, TSearchParameters> FindVacancy(string vacancyReference);
    }
}
