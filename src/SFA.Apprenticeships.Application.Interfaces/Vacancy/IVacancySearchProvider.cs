namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using Domain.Entities.Vacancy;
    using Domain.Entities.Location;
    using Search;

    public interface IVacancySearchProvider
    {
        SearchResults<VacancySummary> FindVacancies(Location location, int radius);
    }
}
