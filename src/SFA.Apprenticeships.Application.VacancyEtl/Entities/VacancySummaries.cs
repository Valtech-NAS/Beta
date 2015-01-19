namespace SFA.Apprenticeships.Application.VacancyEtl.Entities
{
    using System.Collections.Generic;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;

    public class VacancySummaries
    {
        public VacancySummaries(IEnumerable<ApprenticeshipSummary> apprenticeshipSummaries,
            IEnumerable<TraineeshipSummary> traineeshipSummaries)
        {
            ApprenticeshipSummaries = apprenticeshipSummaries;
            TraineeshipSummaries = traineeshipSummaries;
        }

        public IEnumerable<ApprenticeshipSummary> ApprenticeshipSummaries { get; private set; }

        public IEnumerable<TraineeshipSummary> TraineeshipSummaries { get; private set; }
    }
}