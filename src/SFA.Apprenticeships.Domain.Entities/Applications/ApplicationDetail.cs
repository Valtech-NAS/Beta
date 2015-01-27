namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;
    using Candidates;
    using Users;
    using Vacancies;

    public abstract class ApplicationDetail : BaseEntity
    {
        protected ApplicationDetail()
        {
            CandidateDetails = new RegistrationDetails();
            CandidateInformation = new ApplicationTemplate();
            Status = ApplicationStatuses.Unknown;
            VacancyStatus = VacancyStatuses.Unknown;
        }

        public VacancyStatuses VacancyStatus { get; set; }

        public ApplicationStatuses Status { get; set; }
        
        public bool IsArchived { get; set; }

        public DateTime? DateApplied { get; set; }
        
        public Guid CandidateId { get; set; }

        public int LegacyApplicationId { get; set; }
        // temporary "weak link" to legacy application record (could be via an index)

        public RegistrationDetails CandidateDetails { get; set; }

        public ApplicationTemplate CandidateInformation { get; set; }

        public string AdditionalQuestion1Answer { get; set; }

        public string AdditionalQuestion2Answer { get; set; }
    }
}