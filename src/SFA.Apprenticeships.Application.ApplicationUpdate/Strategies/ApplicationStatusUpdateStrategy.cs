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

        public void Update(ApplicationDetail application, ApplicationStatusSummary applicationStatusSummary)
        {
            // invoked because the status of the application / vacancy has changed
            Logger.Info("Updating status of application '{0}' from '{1}' to '{2}' for candidate {3}",
                application.EntityId, 
                application.Status, 
                applicationStatusSummary.ApplicationStatus, 
                application.CandidateDetails.EmailAddress);

            // note, this flow will be extended to include a call to outbound communication later (when we do notifications)
            // note, may subsequently consolidate status updates for a candidate (when we do notifications) but may be done in another component

            // TODO: DEBT: AG: this block of code is duplicated in ApplicationStatusUpdater.
            var updated = false;

            if (application.Status != applicationStatusSummary.ApplicationStatus)
            {
                application.Status = applicationStatusSummary.ApplicationStatus;

                // Application status has changed, ensure it appears on the candidate's dashboard.
                application.IsArchived = false;

                updated = true;
            }

            if (application.LegacyApplicationId != applicationStatusSummary.LegacyApplicationId)
            {
                // Ensure the application is linked to the legacy application.
                application.LegacyApplicationId = applicationStatusSummary.LegacyApplicationId;
                updated = true;
            }

            if (application.Vacancy.ClosingDate != applicationStatusSummary.ClosingDate)
            {
                application.Vacancy.ClosingDate = applicationStatusSummary.ClosingDate;
                updated = true;
            }

            if (application.UnsuccessfulReason != applicationStatusSummary.UnsuccessfulReason)
            {
                application.UnsuccessfulReason = applicationStatusSummary.UnsuccessfulReason;
                updated = true;
            }

            if (updated)
            {
                _applicationWriteRepository.Save(application);
            }
        }
    }
}
