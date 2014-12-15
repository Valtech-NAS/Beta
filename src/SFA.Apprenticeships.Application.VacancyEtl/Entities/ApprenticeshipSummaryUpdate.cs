namespace SFA.Apprenticeships.Application.VacancyEtl.Entities
{
    using System;
    using Domain.Entities.Vacancies.Apprenticeships;

    /// <summary>
    /// Adds update reference required for tidy up process
    /// </summary>
    public class ApprenticeshipSummaryUpdate : ApprenticeshipSummary, IVacancyUpdate
    {
        public DateTime ScheduledRefreshDateTime { get; set; }
    }
}
