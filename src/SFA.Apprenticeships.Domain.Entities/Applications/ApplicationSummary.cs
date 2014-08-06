namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;

    public class ApplicationSummary
    {
        public Guid ApplicationId { get; set; }

        public int LegacyVacancyId { get; set; }

        public string Title { get; set; }

        public ApplicationStatuses Status { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime? DateApplied { get; set; }
    }
}
