namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using CuttingEdge.Conditions;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using Interfaces.Search;
    using Interfaces.Vacancies;
    using NLog;
    using ErrorCodes = Interfaces.Vacancies.ErrorCodes;

    public class VacancySearchService<TVacancySummaryResponse, TVacancyDetail> : IVacancySearchService<TVacancySummaryResponse, TVacancyDetail>
        where TVacancySummaryResponse : VacancySummary
        where TVacancyDetail : VacancyDetail
    {
        private const string MessageFormat =
            "Keywords:{0}, Location:{1}, PageNumber:{2}, PageSize{3}, SearchRadius:{4}, SortType:{5}, LocationType:{6}";

        private const string CallingMessageFormat =
            "Calling VacancySearchService with the following parameters; " + MessageFormat;

        private const string FailedMessageFormat =
            "Vacancy search failed for the following parameters; " + MessageFormat;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancySearchProvider<TVacancySummaryResponse> _vacancySearchProvider;
        private readonly IVacancyDataProvider<TVacancyDetail> _vacancyDataProvider;

        public VacancySearchService(IVacancySearchProvider<TVacancySummaryResponse> vacancySearchProvider, IVacancyDataProvider<TVacancyDetail> vacancyDataProvider)
        {
            _vacancySearchProvider = vacancySearchProvider;
            _vacancyDataProvider = vacancyDataProvider;
        }

        public SearchResults<TVacancySummaryResponse> Search(SearchParameters parameters)
        {
            Condition.Requires(parameters).IsNotNull();
            Condition.Requires(parameters.SearchRadius).IsGreaterOrEqual(0);
            Condition.Requires(parameters.PageNumber).IsGreaterOrEqual(1);
            Condition.Requires(parameters.PageSize).IsGreaterOrEqual(1);

            var enterMmessage = GetLoggerMessage(CallingMessageFormat, parameters);
            _logger.Debug(enterMmessage);

            try
            {
                return _vacancySearchProvider.FindVacancies(parameters);
            }
            catch (Exception e)
            {
                var message = GetLoggerMessage(FailedMessageFormat, parameters);
                _logger.Debug(message, e);
                throw new CustomException(message, e, ErrorCodes.VacanciesSearchFailed);
            }
        }

        public TVacancyDetail GetVacancyDetails(int vacancyId)
        {
            Condition.Requires(vacancyId).IsGreaterOrEqual(1);

            _logger.Debug("Calling VacancyDataProvider to get vacancy details for user {0}.", vacancyId);

            try
            {
                return _vacancyDataProvider.GetVacancyDetails(vacancyId);
            }
            catch (Exception e)
            {
                var message = string.Format("Get vacancy failed for vacancy {0}.", vacancyId);
                _logger.Debug(message, e);
                throw new CustomException(message, e, ErrorCodes.GetVacancyDetailsFailed);
            }
        }

        private static string GetLoggerMessage(string message, SearchParameters parameters)
        {
            return string.Format(message, parameters.Keywords, parameters.Location,
                parameters.PageNumber, parameters.PageSize, parameters.SearchRadius, parameters.SortType,
                parameters.VacancyLocationType);
        }
    }
}