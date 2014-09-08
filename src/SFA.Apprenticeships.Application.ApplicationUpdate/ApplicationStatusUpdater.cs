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

        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly IApplicationReadRepository _applicationReadRepository;

        public ApplicationStatusUpdater(
            IApplicationWriteRepository applicationWriteRepository,
            IApplicationReadRepository applicationReadRepository)
        {
            _applicationWriteRepository = applicationWriteRepository;
            _applicationReadRepository = applicationReadRepository;
        }

        public void Update(Candidate candidate, IEnumerable<ApplicationStatusSummary> applicationStatuses)
        {
            // For the specified candidate, update the application repo for any of the status updates
            // passed in (if they're different).
            foreach (var applicationStatus in applicationStatuses)
            {
                var status = applicationStatus;

                var applicationDetail = _applicationReadRepository.GetForCandidate(
                    candidate.EntityId, each => each.LegacyApplicationId == status.LegacyApplicationId);

                if (applicationDetail != null)
                {
                    var updated = false;

                    // TODO: US154: do we need to check vacancy status too and then derive application status? 
                    // ...or does the the app status already reflect the vacancy status? (e.g. if withdrawn, etc)
                    // check with legacy team.
                    if (applicationDetail.Status != status.ApplicationStatus)
                    {
                        applicationDetail.Status = status.ApplicationStatus;
                        updated = true;
                    }

                    // TODO: AG: added subject to review to ensure we have the latest view of the ClosingDate in Exemplar database.
                    // TODO: AG: map other fields in ApplicationStatusSummary? E.g. UnsuccessfulReason, VacancyStatus?
                    if (applicationDetail.Vacancy.ClosingDate != status.ClosingDate)
                    {
                        applicationDetail.Vacancy.ClosingDate = status.ClosingDate;
                        updated = true;
                    }

                    if (updated)
                    {
                        _applicationWriteRepository.Save(applicationDetail);
                    }
                }
                else
                {
                    // log warning as we need to update an application that isn't in the repo
                    Logger.Warn("Unable to find/update application with legacy ID \"{0}\".", status.LegacyApplicationId);
                }
            }
        }
    }
}
