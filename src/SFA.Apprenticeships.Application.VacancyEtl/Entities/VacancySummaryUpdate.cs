namespace SFA.Apprenticeships.Application.VacancyEtl.Entities
{
    using System;
    using Domain.Entities.Vacancy;

    /// <summary>
    /// Adds update reference required for tidy up process
    /// </summary>
    public class VacancySummaryUpdate : VacancySummary
    {
        public Guid UpdateReference { get; set; }
    }
}
