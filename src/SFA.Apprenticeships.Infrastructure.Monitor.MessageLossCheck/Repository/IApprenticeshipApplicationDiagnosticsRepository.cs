namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Repository
{
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Entities;

    public interface IApprenticeshipApplicationDiagnosticsRepository
    {
        IEnumerable<ApprenticeshipApplicationDetail> GetApplicationsForValidCandidatesWithUnsetLegacyId();

        IEnumerable<CandidateApprenticeshipApplicationDetail> GetSubmittedApplicationsWithUnsetLegacyId();

        IEnumerable<string> GetDraftApplicationVacancyIds();

        void UpdateApplicationStatus(ApprenticeshipApplicationDetail applicationDetail, ApplicationStatuses newApplicationStatus);

        void UpdateLegacyApplicationId(ApprenticeshipApplicationDetail applicationDetail, int legacyApplicationId);
    }
}