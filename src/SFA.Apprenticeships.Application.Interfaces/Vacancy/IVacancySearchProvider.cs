namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using Domain.Entities.Location;
    using Search;

    public interface IVacancySearchProvider
    {
        SearchResults<VacancySummaryResponse> FindVacancies(string keywords, 
                                                            Location location, 
                                                            int pageNumber, 
                                                            int pageSize, 
                                                            int searchRadius,
                                                            VacancySortType sortType);
    }
}
