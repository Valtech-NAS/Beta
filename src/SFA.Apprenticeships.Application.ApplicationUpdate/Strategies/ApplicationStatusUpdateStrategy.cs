namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using System;
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
                applicationStatusSummary.ApplicationId, 
                application.Status, 
                applicationStatusSummary.ApplicationStatus, 
                application.CandidateDetails.EmailAddress);

            // note, this flow will be extended to include a call to outbound communication later (when we do notifications)
            // note, may subsequently consolidate status updates for a candidate (when we do notifications) but may be done in another component

            application.Status = applicationStatusSummary.ApplicationStatus;
            application.IsArchived = false; // note, this ensures the application will be become visible on user's dashboard if hidden

            _applicationWriteRepository.Save(application);
        }
    }
}
