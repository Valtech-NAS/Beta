namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using System;
    using Search;
    using Domain.Entities.Locations;

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
