namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;
    using Vacancies;

    public abstract class ApplicationSummary
    {
        public Guid ApplicationId { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public int LegacyVacancyId { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public DateTime ClosingDate { get; set; }

        public bool IsArchived { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime? DateApplied { get; set; }
    }
}