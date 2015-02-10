namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Extensions;
    using Interfaces.Logging;

    public class ApplicationStatusUpdateStrategy : IApplicationStatusUpdateStrategy
    {
        private readonly ILogService _logger;

        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly IMessageBus _bus;

        public ApplicationStatusUpdateStrategy(
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository, 
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository, 
            IMessageBus bus,
            ILogService logger)
        {
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _bus = bus;
            _logger = logger;
        }

        public void Update(ApprenticeshipApplicationDetail apprenticeshipApplication, ApplicationStatusSummary applicationStatusSummary)
        {
            // invoked because the status of the apprenticeshipApplication / vacancy has changed
            _logger.Info(
                "Updating status of apprenticeship application '{0}' for vacancy '{1}' from '{2}' to '{3}' for candidate {4}",
                apprenticeshipApplication.EntityId,
                applicationStatusSummary.LegacyVacancyId,
                apprenticeshipApplication.Status,
                applicationStatusSummary.ApplicationStatus,
                apprenticeshipApplication.CandidateDetails.EmailAddress);

            // note, this flow will be extended to include a call to outbound communication later (when we do notifications)
            // note, may subsequently consolidate status updates for a candidate (when we do notifications) but may be done in another component

            if (apprenticeshipApplication.UpdateApprenticeshipApplicationDetail(applicationStatusSummary))
            {
                _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplication);

                // note, to force vacancy status updates for users with draft applications for this vacancy
                var vacancyStatusSummary = new VacancyStatusSummary
                {
                    LegacyVacancyId = applicationStatusSummary.LegacyVacancyId,
                    ClosingDate = applicationStatusSummary.ClosingDate,
                    VacancyStatus = applicationStatusSummary.VacancyStatus
                };

                _bus.PublishMessage(vacancyStatusSummary);
            }
        }

        public void Update(TraineeshipApplicationDetail traineeeshipApplication, ApplicationStatusSummary applicationStatusSummary)
        {
            // invoked because the status of the apprenticeshipApplication / vacancy has changed
            _logger.Info(
                "Updating status of traineeship application '{0}' for vacancy '{1}' from '{2}' to '{3}' for candidate {4}",
                traineeeshipApplication.EntityId,
                applicationStatusSummary.LegacyVacancyId,
                traineeeshipApplication.Status,
                applicationStatusSummary.ApplicationStatus,
                traineeeshipApplication.CandidateDetails.EmailAddress);

            // note, this flow will be extended to include a call to outbound communication later (when we do notifications)
            // note, may subsequently consolidate status updates for a candidate (when we do notifications) but may be done in another component

            if (traineeeshipApplication.UpdateTraineeshipApplicationDetail(applicationStatusSummary))
            {
                _traineeshipApplicationWriteRepository.Save(traineeeshipApplication);
            }
        }
    }
}
