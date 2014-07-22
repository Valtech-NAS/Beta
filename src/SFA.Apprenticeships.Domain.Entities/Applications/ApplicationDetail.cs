namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;
    using Candidates;
    using Vacancies;

    public class ApplicationDetail : BaseEntity
    {
        public ApplicationDetail()
        {
            CandidateDetails = new RegistrationDetails();
            CandidateInformation = new ApplicationTemplate();
            Status = ApplicationStatuses.Unknown;
        }

        public ApplicationStatuses Status { get; set; }

        public DateTime? DateApplied { get; set; }

        public bool IsArchived { get; set; }

        public Guid CandidateId { get; set; }

        public int LegacyApplicationId { get; set; } // temporary "weak link" to legacy application record (could be via an index)

        public VacancySummary Vacancy { get; set; }

        public RegistrationDetails CandidateDetails { get; set; }

        public ApplicationTemplate CandidateInformation { get; set; }
    }
}
