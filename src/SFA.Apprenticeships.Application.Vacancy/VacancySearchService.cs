using SFA.Apprenticeships.Application.Interfaces.Vacancies;
using SFA.Apprenticeships.Domain.Entities.Locations;

namespace SFA.Apprenticeships.Application.Vacancy
{
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Domain.Interfaces.Logging;
    using Interfaces.Search;

    public class VacancySearchService : IVacancySearchService
    {
        private readonly IVacancySearchProvider _vacancySearchProvider;
        private readonly ILoggingService _loggingService;

        public VacancySearchService(IVacancySearchProvider vacancySearchProvider, ILoggingService loggingService)
        {
            _vacancySearchProvider = vacancySearchProvider;
            _loggingService = loggingService;
        }

        public SearchResults<VacancySummaryResponse> Search(string keywords, 
                                                            Location location, 
                                                            int pageNumber, 
                                                            int pageSize, 
                                                            int searchRadius,
                                                            VacancySortType sortType)
        {
            Condition.Requires(location, "location").IsNotNull();
            Condition.Requires(searchRadius, "searchRadius").IsGreaterOrEqual(0);
            Condition.Requires(pageNumber, "pageNumber").IsGreaterOrEqual(1);
            Condition.Requires(pageSize, "pageSize").IsGreaterOrEqual(1);

            var vacancies = _vacancySearchProvider.FindVacancies(keywords, location, pageNumber, pageSize, searchRadius, sortType);

            return vacancies;
        }
    }
}
