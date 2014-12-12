namespace SFA.Apprenticeships.Application.VacancyEtl.Entities
{
    using System;
    using Domain.Entities.Vacancies.Traineeships;

    /// <summary>
    /// Adds update reference required for tidy up process
    /// </summary>
    public class TraineeshipSummaryUpdate : TraineeshipSummary, IVacancyUpdate
    {
        public DateTime ScheduledRefreshDateTime { get; set; }
    }
}
