namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;
    using Candidates;

    public class ApplicationDetail : BaseEntity
    {
        // TODO: DONTKNOW: ApplicationDetail, status, vacancy info, etc.

        public ApplicationDetail()
        {
            CandidateDetails = new RegistrationDetails();
            CandidateInformation = new ApplicationTemplate();
            Status = ApplicationStatuses.Unknown;
        }

        public ApplicationStatuses Status { get; set; }

        public Guid CandidateId { get; set; }

        public int LegacyApplicationId { get; set; } // temporary "weak link" to legacy application record

        public RegistrationDetails CandidateDetails { get; set; }

        public ApplicationTemplate CandidateInformation { get; set; }
    }
}
