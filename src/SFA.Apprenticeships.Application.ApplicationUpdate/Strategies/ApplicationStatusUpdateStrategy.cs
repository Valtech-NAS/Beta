namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Extensions;
    using Interfaces.Logging;

    public class ApplicationStatusUpdateStrategy : IApplicationStatusUpdateStrategy
    {
        private readonly ILogService _logger;

        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;

        public ApplicationStatusUpdateStrategy(
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository, 
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository, 
            ILogService logger)
        {
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _logger = logger;
        }

        public void Update(ApprenticeshipApplicationDetail apprenticeshipApplication, ApplicationStatusSummary applicationStatusSummary)
        {
            var originalLegacyApplicationId = apprenticeshipApplication.LegacyApplicationId;
            var originalStatus = apprenticeshipApplication.Status;
            var originalVacancyStatus = apprenticeshipApplication.VacancyStatus;
            var originalClosingDate = apprenticeshipApplication.Vacancy.ClosingDate;
            var originalUnsuccessfulReason = apprenticeshipApplication.UnsuccessfulReason;

            // invoked because the status of the apprenticeshipApplication / vacancy *may* have changed
            if (apprenticeshipApplication.UpdateApprenticeshipApplicationDetail(applicationStatusSummary))
            {
                // note, this flow will be extended to include a call to outbound communication later (when we do notifications)
                // note, may subsequently consolidate status updates for a candidate (when we do notifications) but may be done in another component
                const string format =
                    "Updating apprenticeship application (id='{0}', vacancy id='{1}', candidate='{2})" +
                    " from legacy application id='{3}' to '{4}'," +
                    " application status='{5}' to '{6}'," +
                    " vacancy status='{7}' to '{8}'," +
                    " closing date='{9}' to '{10}'," +
                    " unsuccessful reason='{11}' to '{12}'";

                _logger.Info(
                    format,
                    apprenticeshipApplication.EntityId, // 0
                    apprenticeshipApplication.Vacancy.Id, // 1
                    apprenticeshipApplication.CandidateDetails.EmailAddress, // 2

                    originalLegacyApplicationId, // 3
                    applicationStatusSummary.LegacyApplicationId, // 4

                    originalStatus, // 5
                    applicationStatusSummary.ApplicationStatus, // 6

                    originalVacancyStatus, // 7
                    applicationStatusSummary.VacancyStatus, // 8

                    originalClosingDate, // 9
                    applicationStatusSummary.ClosingDate, // 10

                    originalUnsuccessfulReason, // 11
                    applicationStatusSummary.UnsuccessfulReason); // 12

                _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplication);
            }
        }

        public void Update(TraineeshipApplicationDetail traineeeshipApplication, ApplicationStatusSummary applicationStatusSummary)
        {
            var originalLegacyApplicationId = traineeeshipApplication.LegacyApplicationId;
            var originalStatus = traineeeshipApplication.Status;
            var originalVacancyStatus = traineeeshipApplication.VacancyStatus;
            var originalClosingDate = traineeeshipApplication.Vacancy.ClosingDate;

            //todo: 1.6: remove this? we won't ever receive these updates while integrating with the legacy system for traineeships
            // invoked because the status of the apprenticeshipApplication / vacancy *may* have changed
            if (traineeeshipApplication.UpdateTraineeshipApplicationDetail(applicationStatusSummary))
            {
                // note, this flow will be extended to include a call to outbound communication later (when we do notifications)
                // note, may subsequently consolidate status updates for a candidate (when we do notifications) but may be done in another component
                const string format =
                    "Updating traineeship application (id='{0}', vacancy id='{1}', candidate='{2})" +
                    " from legacy application id='{3}' to '{4}'," +
                    " application status='{5}' to '{6}'," +
                    " vacancy status='{7}' to '{8}'," +
                    " closing date='{9}' to '{10}'";

                _logger.Info(
                    format,
                    traineeeshipApplication.EntityId, // 0
                    traineeeshipApplication.Vacancy.Id, // 1
                    traineeeshipApplication.CandidateDetails.EmailAddress, // 2

                    originalLegacyApplicationId, // 3
                    applicationStatusSummary.LegacyApplicationId, // 4

                    originalStatus, // 5
                    applicationStatusSummary.ApplicationStatus, // 6

                    originalVacancyStatus, // 7
                    applicationStatusSummary.VacancyStatus, // 8

                    originalClosingDate, // 9
                    applicationStatusSummary.ClosingDate); // 10

                _traineeshipApplicationWriteRepository.Save(traineeeshipApplication);
            }
        }
    }
}
