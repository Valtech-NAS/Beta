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
                var message = string.Format("Vacancy search failed. Keywords:{0}, Location:{1}," +
                                            "PageNumber:{2}, PageSize{3}, SearchRadius:{4}," +
                                            "SortType:{5}", keywords, location, pageNumber, pageSize,
                                            searchRadius, sortType);
                throw new Domain.Entities.Exceptions.CustomException(
                    message, e, ErrorCodes.VacanciesSearchFailed);
            }
        }
    }
}
