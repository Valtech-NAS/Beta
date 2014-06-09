namespace SFA.Apprenticeships.Application.Vacancy
{
    using CuttingEdge.Conditions;
    using Domain.Entities.Location;
    using Domain.Entities.Vacancy;
    using Domain.Interfaces.Logging;
    using Interfaces.Vacancy;
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

        public SearchResults<VacancySummary> Search(Location location, int searchRadius)
        {
            Condition.Requires(location, "location").IsNotNull();
            Condition.Requires(searchRadius, "searchRadius").IsGreaterOrEqual(0);

            var vacancies = _vacancySearchProvider.FindVacancies(location, searchRadius);

            return vacancies;
        }
    }
}
