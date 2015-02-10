namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;

    public class ApplicationVacancyUpdater : IApplicationVacancyUpdater
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;

        public ApplicationVacancyUpdater(
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository)
        {
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
        }

        public void Update(Guid candidateId, int vacancyId, VacancyDetail vacancyDetail)
        {
            // Try apprenticeships first, the majority should be apprenticeships.
            // Note that it is possible for a candidate to have no application for this vacancy.
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
            }
        }
    }
}