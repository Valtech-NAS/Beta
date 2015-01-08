namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using NLog;

    public class ApplicationStatusUpdater : IApplicationStatusUpdater
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;

        public ApplicationStatusUpdater(
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository)
        {
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
        }

        public void Update(Candidate candidate, IEnumerable<ApplicationStatusSummary> applicationStatuses)
        {
            // For the specified candidate, update the application repo for any of the status updates
            // passed in (if they're different).
            foreach (var applicationStatus in applicationStatuses)
            {
                var legacyVacancyId  = applicationStatus.LegacyVacancyId;

                var applicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidate.EntityId, legacyVacancyId);

                // TODO: DEBT: AG: this block of code is duplicated in ApplicationStatusUpdateStrategy.
                if (applicationDetail != null)
                {
                    var updated = false;

                    if (applicationDetail.Status != applicationStatus.ApplicationStatus)
                    {
                        applicationDetail.Status = applicationStatus.ApplicationStatus;
                        
                        // Application status has changed, ensure it appears on the candidate's dashboard.
                        applicationDetail.IsArchived = false;

                        updated = true;
                    }

                    if (applicationDetail.LegacyApplicationId != applicationStatus.LegacyApplicationId)
                    {
                        // Ensure the application is linked to the legacy application.
                        applicationDetail.LegacyApplicationId = applicationStatus.LegacyApplicationId;
                        updated = true;
                    }

                    if (applicationDetail.Vacancy.ClosingDate != applicationStatus.ClosingDate)
                    {
                        applicationDetail.Vacancy.ClosingDate = applicationStatus.ClosingDate;
                        updated = true;
                    }

                    if (applicationDetail.UnsuccessfulReason != applicationStatus.UnsuccessfulReason)
                    {
                        applicationDetail.UnsuccessfulReason = applicationStatus.UnsuccessfulReason;
                        updated = true;
                    }

                    if (updated)
                    {
                        _apprenticeshipApplicationWriteRepository.Save(applicationDetail);
                    }
                }
                else
                {
                    Logger.Warn("Unable to find application with legacy ID \"{0}\".", applicationStatus.LegacyApplicationId);
                }
            }
        }
    }
}
