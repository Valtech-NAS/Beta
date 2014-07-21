namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;

    public class Candidate : BaseEntity
    {
        public Candidate()
        {
            RegistrationDetails = new RegistrationDetails();
            ApplicationTemplate = new ApplicationTemplate();
        }

        public int LegacyCandidateId { get; set; } // temporary "weak link" to legacy candidate record (could be via an index)

        public RegistrationDetails RegistrationDetails { get; set; }

        public ApplicationTemplate ApplicationTemplate { get; set; }
    }
}
