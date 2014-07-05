namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;
    using Candidates;

    public class ApplicationDetail : BaseEntity
    {
        //todo: ApplicationDetail, status, vacancy info, etc.

        public ApplicationDetail()
        {
            CandidateDetails = new PersonalDetails();
            CandidateInformation = new ApplicationInformation();
        }

        public Guid CandidateId { get; set; }

        public int LegacyApplicationId { get; set; } // temporary "weak link" to legacy application record

        public PersonalDetails CandidateDetails { get; set; }

        public ApplicationInformation CandidateInformation { get; set; }
    }
}
