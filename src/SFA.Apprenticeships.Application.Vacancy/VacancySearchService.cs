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

    public class VacancySearchService<TVacancySummaryResponse> : IVacancySearchService<TVacancySummaryResponse>
        where TVacancySummaryResponse : VacancySummary
    {
        private const string MessageFormat =
            "Keywords:{0}, Location:{1}, PageNumber:{2}, PageSize{3}, SearchRadius:{4}, SortType:{5}, LocationType:{6}";

        private const string CallingMessageFormat =
            "Calling VacancySearchService with the following parameters; " + MessageFormat;

        private const string FailedMessageFormat =
            "Vacancy search failed for the following parameters; " + MessageFormat;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancySearchProvider<TVacancySummaryResponse> _vacancySearchProvider;

        public VacancySearchService(IVacancySearchProvider<TVacancySummaryResponse> vacancySearchProvider)
        {
            _vacancySearchProvider = vacancySearchProvider;
        }

        public SearchResults<TVacancySummaryResponse> Search(SearchParameters parameters)
        {
            Condition.Requires(parameters).IsNotNull();
            Condition.Requires(parameters.SearchRadius).IsGreaterOrEqual(0);
            Condition.Requires(parameters.PageNumber).IsGreaterOrEqual(1);
            Condition.Requires(parameters.PageSize).IsGreaterOrEqual(1);

            var enterMmessage = GetLoggerMessage(CallingMessageFormat, parameters);
            Logger.Debug(enterMmessage);

            try
            {
                return _vacancySearchProvider.FindVacancies(parameters);
            }
            catch (Exception e)
            {
                var message = GetLoggerMessage(FailedMessageFormat, parameters);
                Logger.Debug(message, e);
                throw new CustomException(message, e, ErrorCodes.VacanciesSearchFailed);
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