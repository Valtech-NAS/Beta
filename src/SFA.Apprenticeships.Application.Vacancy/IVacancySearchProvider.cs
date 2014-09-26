namespace SFA.Apprenticeships.Application.Vacancy
{
    using Interfaces.Search;
    using Interfaces.Vacancies;

    public interface IVacancySearchProvider
    {
        SearchResults<VacancySummaryResponse> FindVacancies(SearchParameters parameters);
    }
}