namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Extensions;
    using Interfaces.Logging;

    public class ApplicationStatusUpdater : IApplicationStatusUpdater
    {
        private readonly ILogService _logger;

        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;

        public ApplicationStatusUpdater(
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository, ILogService logger)
        {
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _logger = logger;
        }

        public void Update(Candidate candidate, IEnumerable<ApplicationStatusSummary> applicationStatuses)
        {
            // For the specified candidate, update the application repo for any of the status updates
            // passed in (if they're different).
            foreach (var applicationStatusSummary in applicationStatuses)
            {
                var legacyVacancyId = applicationStatusSummary.LegacyVacancyId;

                // Try apprenticeships first, the majority should be apprenticeships
                var apprenticeshipApplication = _apprenticeshipApplicationReadRepository.GetForCandidate(candidate.EntityId, legacyVacancyId);

                if (apprenticeshipApplication != null)
                {
                    if (apprenticeshipApplication.UpdateApprenticeshipApplicationDetail(applicationStatusSummary))
                    {
                        _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplication);
                    }
                    return;
                }

                var traineeshipApplication = _traineeshipApplicationReadRepository.GetForCandidate(candidate.EntityId, legacyVacancyId);

                if (traineeshipApplication != null)
                {
                    if (traineeshipApplication.UpdateTraineeshipApplicationDetail(applicationStatusSummary))
                    {
                        _traineeshipApplicationWriteRepository.Save(traineeshipApplication);
                    }
                    return;
                }

                _logger.Warn("Unable to find apprenticeship or traineeship application with legacy ID \"{0}\".", applicationStatusSummary.LegacyApplicationId);
            }
        }
    }
}
