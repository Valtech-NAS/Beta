namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Location;
    using Domain.Entities.Vacancy;
    using Domain.Interfaces.Services.Logging;
    using Interfaces.Vacancy;

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

        public IEnumerable<VacancySummary> Search(Location location, int searchRadius)
        {
            Condition.Requires(location, "location").IsNotNull();
            Condition.Requires(searchRadius, "searchRadius").IsGreaterOrEqual(0);

            var vacancies = _vacancySearchProvider.FindVacancies(location, searchRadius);

            return vacancies;
        }
    }
}
