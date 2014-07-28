namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using NLog;

    public class ApplicationStatusUpdater : IApplicationStatusUpdater
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly IApplicationReadRepository _applicationReadRepository;

        public ApplicationStatusUpdater(IApplicationWriteRepository applicationWriteRepository, IApplicationReadRepository applicationReadRepository)
        {
            _applicationWriteRepository = applicationWriteRepository;
            _applicationReadRepository = applicationReadRepository;
        }

        public void Update(Candidate candidate, IEnumerable<ApplicationStatusSummary> applicationStatuses)
        {
            // for the candidate, update the application repo for any of the status updates passed in (if they're different)
            foreach (var applicationStatus in applicationStatuses)
            {
                var status = applicationStatus;
                var application = _applicationReadRepository.GetForCandidate(candidate.EntityId, a => a.LegacyApplicationId == status.LegacyApplicationId);

                if (application != null)
                {
                    //todo: need to check vacancy status too or will app status reflect this already? check with legacy team
                    if (application.Status != status.ApplicationStatus)
                    {
                        _applicationWriteRepository.Save(application);
                    }
                }
                else
                {
                    // log warning as we need to update an application that isn't in the repo
                    Logger.Warn("Unable to find/update application with legacy ID '{0}'", status.LegacyApplicationId);
                }
            }
        }
    }
}
