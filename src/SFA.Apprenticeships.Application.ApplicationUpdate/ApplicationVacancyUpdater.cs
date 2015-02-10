namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;

    public class ApplicationVacancyUpdater : IApplicationVacancyUpdater
    {
        private readonly ILogService _logger;

        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;

        public ApplicationVacancyUpdater(
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

        public void Update(Guid candidateId, int vacancyId, VacancyDetail vacancyDetail)
        {
            // Try apprenticeships first, the majority should be apprenticeships.
            var apprenticeshipApplication = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);
            var updated = false;

            if (apprenticeshipApplication != null)
            {
                if (apprenticeshipApplication.VacancyStatus != vacancyDetail.VacancyStatus)
                {
                    apprenticeshipApplication.VacancyStatus = vacancyDetail.VacancyStatus;
                    updated = true;
                }

                if (apprenticeshipApplication.Vacancy.ClosingDate != vacancyDetail.ClosingDate)
                {
                    apprenticeshipApplication.Vacancy.ClosingDate = vacancyDetail.ClosingDate;
                    updated = true;
                }

                if (updated)
                {
                    _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplication);                    
                }
            }
            else
            {
                var traineeshipApplication = _traineeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);

                if (traineeshipApplication != null)
                {
                    if (traineeshipApplication.VacancyStatus != vacancyDetail.VacancyStatus)
                    {
                        traineeshipApplication.VacancyStatus = vacancyDetail.VacancyStatus;
                        updated = true;
                    }

                    if (traineeshipApplication.Vacancy.ClosingDate != vacancyDetail.ClosingDate)
                    {
                        traineeshipApplication.Vacancy.ClosingDate = vacancyDetail.ClosingDate;
                        updated = true;
                    }

                    if (updated)
                    {
                        _traineeshipApplicationWriteRepository.Save(traineeshipApplication);
                    }
                }
                else
                {
                    //todo: shouldn't warn as may have been called for a vacancy the candidate doesn't have an application for
                    _logger.Warn(
                        "Unable to find apprenticeship or traineeship application for candiate ID {0} with legacy vacancy ID \"{1}\".",
                        candidateId, vacancyId);
                }
            }
        }
    }
}