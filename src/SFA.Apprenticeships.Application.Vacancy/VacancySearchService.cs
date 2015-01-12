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

    public class VacancySearchService<TVacancySummaryResponse, TVacancyDetail, TSearchParameters> : IVacancySearchService<TVacancySummaryResponse, TVacancyDetail, TSearchParameters>
        where TVacancySummaryResponse : VacancySummary
        where TVacancyDetail : VacancyDetail
        where TSearchParameters : SearchParametersBase
    {
        private const string CallingMessageFormat = "Calling VacancySearchService with the following parameters; {0}";

        private const string FailedMessageFormat = "Vacancy search failed for the following parameters; {0}";

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancySearchProvider<TVacancySummaryResponse, TSearchParameters> _vacancySearchProvider;
        private readonly IVacancyDataProvider<TVacancyDetail> _vacancyDataProvider;

        public VacancySearchService(IVacancySearchProvider<TVacancySummaryResponse, TSearchParameters> vacancySearchProvider, IVacancyDataProvider<TVacancyDetail> vacancyDataProvider)
        {
            _vacancySearchProvider = vacancySearchProvider;
            _vacancyDataProvider = vacancyDataProvider;
        }

        public SearchResults<TVacancySummaryResponse> Search(TSearchParameters parameters)
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

            _logger.Debug("Calling VacancyDataProvider to get vacancy details for vacancy {0}.", vacancyId);

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

        private static string GetLoggerMessage(string message, SearchParametersBase parameters)
        {
            return string.Format(message, parameters);
        }
    }
}