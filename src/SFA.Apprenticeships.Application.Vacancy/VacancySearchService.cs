using SFA.Apprenticeships.Domain.Interfaces.Logging;

namespace SFA.Apprenticeships.Application.Vacancy
{
    using CuttingEdge.Conditions;
    using Domain.Entities.Location;
    using Domain.Entities.Vacancy;
    using Interfaces.Vacancy;
    using SFA.Apprenticeships.Application.Interfaces.Search;

    public class VacancySearchService : IVacancySearchService
    {
        private readonly IVacancySearchProvider _vacancySearchProvider;
        private readonly ILoggingService _loggingService;

        public VacancySearchService(IVacancySearchProvider vacancySearchProvider, ILoggingService loggingService)
        {
            Condition.Requires(vacancySearchProvider, "vacancySearchProvider").IsNotNull();
            Condition.Requires(loggingService, "loggingService").IsNotNull();

            _vacancySearchProvider = vacancySearchProvider;
            _loggingService = loggingService;
        }

        public SearchResults<VacancySummary> Search(Location location, int searchRadius)
        {
            Condition.Requires(location, "location").IsNotNull();
            Condition.Requires(searchRadius, "searchRadius").IsGreaterOrEqual(0);

            var vacancies = _vacancySearchProvider.FindVacancies(location, searchRadius);

            return vacancies;
        }
    }
}
