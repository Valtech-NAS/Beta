namespace SFA.Apprenticeships.Application.ApplicationUpdate.Entities
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;

    public class ApplicationStatusSummary
    {
        public Guid ApplicationId { get; set; }

        public int LegacyApplicationId { get; set; }

        // TODO: 1.6: map LegacyCandidateId.
        // public int LegacyCandidateId { get; set; }

        public ApplicationStatuses ApplicationStatus { get; set; }
            
        public int LegacyVacancyId { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public DateTime ClosingDate { get; set; }

        public string UnsuccessfulReason { get; set; } // note: this relates to the vacancy manager rejecting a candidate's application
    }
}
