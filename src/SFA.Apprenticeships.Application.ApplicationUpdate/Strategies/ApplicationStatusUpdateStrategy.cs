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
            Logger.Info("Updating status of apprenticeshipApplication '{0}' from '{1}' to '{2}' for candidate {3}", 
                applicationStatusSummary.ApplicationId, 
                apprenticeshipApplication.Status, 
                applicationStatusSummary.ApplicationStatus, 
                apprenticeshipApplication.CandidateDetails.EmailAddress);

            // note, this flow will be extended to include a call to outbound communication later (when we do notifications)
            // note, may subsequently consolidate status updates for a candidate (when we do notifications) but may be done in another component

            apprenticeshipApplication.Status = applicationStatusSummary.ApplicationStatus;
            apprenticeshipApplication.IsArchived = false; // note, this ensures the apprenticeshipApplication will be become visible on user's dashboard if hidden
            apprenticeshipApplication.LegacyApplicationId = applicationStatusSummary.LegacyApplicationId;

            _applicationWriteRepository.Save(apprenticeshipApplication);
        }
    }
}