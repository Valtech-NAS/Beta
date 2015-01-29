namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using ApplicationUpdate;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using NLog;
    using Vacancy;
    using ErrorCodes = Interfaces.Vacancies.ErrorCodes;

    public class LegacyGetCandidateVacancyDetailStrategy<TVacancyDetail> : ILegacyGetCandidateVacancyDetailStrategy<TVacancyDetail>
        where TVacancyDetail : VacancyDetail
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IVacancyDataProvider<TVacancyDetail> _vacancyDataProvider;
        private readonly IApplicationVacancyStatusUpdater _applicationVacancyStatusUpdater;

        public LegacyGetCandidateVacancyDetailStrategy(
            IVacancyDataProvider<TVacancyDetail> vacancyDataProvider,
            IApplicationVacancyStatusUpdater applicationVacancyStatusUpdater)
        {
            _vacancyDataProvider = vacancyDataProvider;
            _applicationVacancyStatusUpdater = applicationVacancyStatusUpdater;
        }

        public TVacancyDetail GetVacancyDetails(Guid candidateId, int vacancyId)
        {
            Logger.Debug("Calling LegacyGetCandidateVacancyDetailStrategy to get vacancy details for vacancy ID {0} and candidate ID {1}.", vacancyId, candidateId);

            try
            {
                var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

                _applicationVacancyStatusUpdater.Update(candidateId, vacancyId, vacancyDetails.VacancyStatus);

                // TODO: AG: queue latest vacancy status for other candidates.

                return vacancyDetails;
            }
            catch (Exception e)
            {
                var message = string.Format("Get vacancy failed for vacancy ID {0} and candidate ID {1}.", vacancyId, candidateId);

                Logger.Debug(message, e);

                throw new CustomException(message, e, ErrorCodes.GetVacancyDetailsFailed);
            }
        }
    }
}
