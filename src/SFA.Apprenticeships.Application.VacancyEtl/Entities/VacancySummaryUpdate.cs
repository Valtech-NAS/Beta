namespace SFA.Apprenticeships.Application.VacancyEtl.Entities
{
    using System;
    using Domain.Entities.Vacancies;

    /// <summary>
    /// Adds update reference required for tidy up process
    /// </summary>
    public class VacancySummaryUpdate : VacancySummary
    {
        public DateTime ScheduledRefreshDateTime { get; set; }
    }
}
