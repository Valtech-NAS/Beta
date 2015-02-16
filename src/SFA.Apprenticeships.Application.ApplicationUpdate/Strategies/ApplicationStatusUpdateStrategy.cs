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

                    apprenticeshipApplication.LegacyApplicationId, // 3
                    applicationStatusSummary.LegacyApplicationId, // 4

                    apprenticeshipApplication.Status, // 5
                    applicationStatusSummary.ApplicationStatus, // 6

                    apprenticeshipApplication.VacancyStatus, // 7
                    applicationStatusSummary.VacancyStatus, // 8

                    apprenticeshipApplication.Vacancy.ClosingDate, // 9
                    applicationStatusSummary.ClosingDate, // 10

                    apprenticeshipApplication.UnsuccessfulReason, // 11
                    applicationStatusSummary.UnsuccessfulReason); // 12

                _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplication);
            }
        }

        public void Update(TraineeshipApplicationDetail traineeeshipApplication, ApplicationStatusSummary applicationStatusSummary)
        {
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

                    traineeeshipApplication.LegacyApplicationId, // 3
                    applicationStatusSummary.LegacyApplicationId, // 4

                    traineeeshipApplication.Status, // 5
                    applicationStatusSummary.ApplicationStatus, // 6

                    traineeeshipApplication.VacancyStatus, // 7
                    applicationStatusSummary.VacancyStatus, // 8

                    traineeeshipApplication.Vacancy.ClosingDate, // 9
                    applicationStatusSummary.ClosingDate); // 10

                _traineeshipApplicationWriteRepository.Save(traineeeshipApplication);
            }
        }
    }
}
