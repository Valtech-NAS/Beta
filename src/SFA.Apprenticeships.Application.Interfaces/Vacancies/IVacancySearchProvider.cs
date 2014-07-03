using System;
using SFA.Apprenticeships.Application.Interfaces.Search;

namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    public interface IVacancySearchProvider
    {
        SearchResults<VacancySummaryResponse> FindVacancies(string keywords, 
                                                            Domain.Entities.Locations.Location location, 
                                                            int pageNumber, 
                                                            int pageSize, 
                                                            int searchRadius,
                                                            VacancySortType sortType);
    }
}
