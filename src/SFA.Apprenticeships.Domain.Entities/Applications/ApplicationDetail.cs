namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;
    using Candidates;
    using Users;

    public abstract class ApplicationDetail : BaseEntity
    {
        protected ApplicationDetail()
        {
            CandidateDetails = new RegistrationDetails();
            CandidateInformation = new ApplicationTemplate();
        }
        
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