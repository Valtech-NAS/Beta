namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Interfaces.Search;
    using Interfaces.Vacancies;

    public class VacancySearchService : IVacancySearchService
    {
        private readonly IVacancySearchProvider _vacancySearchProvider;

        public VacancySearchService(IVacancySearchProvider vacancySearchProvider)
        {
            _vacancySearchProvider = vacancySearchProvider;
        }

        public SearchResults<VacancySummaryResponse> Search(
            string keywords,
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

            try
            {
                return _vacancySearchProvider.FindVacancies(keywords, location, pageNumber, pageSize, searchRadius, sortType);
            }
            catch (Exception e)
            {
                throw new Domain.Entities.Exceptions.CustomException(
                    "Vacancy search failed.", e, ErrorCodes.VacanciesSearchFailed);
            }
        }
    }
}
