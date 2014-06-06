namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using Domain.Entities.Vacancy;
    using Domain.Entities.Location;
    using SFA.Apprenticeships.Application.Interfaces.Search;

    public interface IVacancySearchProvider
    {
        SearchResults<VacancySummary> FindVacancies(Location location, int radius);
    }
}
