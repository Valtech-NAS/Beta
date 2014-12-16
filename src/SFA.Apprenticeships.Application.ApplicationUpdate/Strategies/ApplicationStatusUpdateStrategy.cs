namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using NLog;

    public class ApplicationStatusUpdateStrategy : IApplicationStatusUpdateStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IApplicationWriteRepository _applicationWriteRepository;

        public ApplicationStatusUpdateStrategy(IApplicationWriteRepository applicationWriteRepository)
        {
            _applicationWriteRepository = applicationWriteRepository;
        }

        public void Update(ApprenticeshipApplicationDetail apprenticeshipApplication, ApplicationStatusSummary applicationStatusSummary)
        {
            // invoked because the status of the apprenticeshipApplication / vacancy has changed
            Logger.Info("Updating status of apprenticeship application '{0}' for vacancy '{1}' from '{2}' to '{3}' for candidate {4}",
                apprenticeshipApplication.EntityId, 
                applicationStatusSummary.LegacyVacancyId,
                apprenticeshipApplication.Status, 
                applicationStatusSummary.ApplicationStatus, 
                apprenticeshipApplication.CandidateDetails.EmailAddress);

            // note, this flow will be extended to include a call to outbound communication later (when we do notifications)
            // note, may subsequently consolidate status updates for a candidate (when we do notifications) but may be done in another component

            // TODO: DEBT: AG: this block of code is duplicated in ApplicationStatusUpdater.
            var updated = false;

            if (apprenticeshipApplication.Status != applicationStatusSummary.ApplicationStatus)
            {
                apprenticeshipApplication.Status = applicationStatusSummary.ApplicationStatus;

                // Application status has changed, ensure it appears on the candidate's dashboard.
                apprenticeshipApplication.IsArchived = false;

                updated = true;
            }

            if (apprenticeshipApplication.LegacyApplicationId != applicationStatusSummary.LegacyApplicationId)
            {
                // Ensure the application is linked to the legacy application.
                apprenticeshipApplication.LegacyApplicationId = applicationStatusSummary.LegacyApplicationId;
                updated = true;
            }

            if (apprenticeshipApplication.Vacancy.ClosingDate != applicationStatusSummary.ClosingDate)
            {
                apprenticeshipApplication.Vacancy.ClosingDate = applicationStatusSummary.ClosingDate;
                updated = true;
            }

            if (apprenticeshipApplication.UnsuccessfulReason != applicationStatusSummary.UnsuccessfulReason)
            {
                apprenticeshipApplication.UnsuccessfulReason = applicationStatusSummary.UnsuccessfulReason;
                updated = true;
            }

            if (updated)
            {
                _applicationWriteRepository.Save(apprenticeshipApplication);
            }
        }
    }
}
