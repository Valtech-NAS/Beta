namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Interfaces.Search;
    using Interfaces.Vacancies;
    using NLog;

    public class VacancySearchService : IVacancySearchService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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
            VacancySortType sortType,
            VacancyLocationType vacancyLocationType)
        {
            Condition.Requires(location, "location").IsNotNull();
            Condition.Requires(searchRadius, "searchRadius").IsGreaterOrEqual(0);
            Condition.Requires(pageNumber, "pageNumber").IsGreaterOrEqual(1);
            Condition.Requires(pageSize, "pageSize").IsGreaterOrEqual(1);

            var enterMmessage =
                string.Format("Calling VacancySearchService to search for a vacancy. Keywords:{0}, Location:{1}," +
                              "PageNumber:{2}, PageSize{3}, SearchRadius:{4}," +
                              "SortType:{5}" + "LocationType:{6}", keywords, location, pageNumber, pageSize,
                    searchRadius, sortType, vacancyLocationType);
            Logger.Debug(enterMmessage);

            try
            {
                return _vacancySearchProvider.FindVacancies(keywords, location, pageNumber, pageSize, searchRadius, sortType, vacancyLocationType);
            }
            catch (Exception e)
            {
                var message = string.Format("Vacancy search failed. Keywords:{0}, Location:{1}," +
                                            "PageNumber:{2}, PageSize{3}, SearchRadius:{4}," +
                                            "SortType:{5}" + "LocationType:{6}", keywords, location, pageNumber, pageSize,
                                            searchRadius, sortType, vacancyLocationType);
                Logger.DebugException(message, e);
                throw new Domain.Entities.Exceptions.CustomException(
                    message, e, ErrorCodes.VacanciesSearchFailed);
            }
        }
    }
}
