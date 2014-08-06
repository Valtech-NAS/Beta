namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using Domain.Entities.Locations;
    using Interfaces.Search;
    using Interfaces.Vacancies;

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
