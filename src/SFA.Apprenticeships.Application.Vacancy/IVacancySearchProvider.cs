namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Interfaces.Search;
    using Interfaces.Vacancies;

    public interface IVacancySearchProvider
    {
        SearchResults<VacancySummaryResponse> FindVacancies(string keywords,
            Location location,
            int pageNumber,
            int pageSize,
            int searchRadius,
            VacancySortType sortType,
            VacancyLocationType vacancyLocationType);
    }
}