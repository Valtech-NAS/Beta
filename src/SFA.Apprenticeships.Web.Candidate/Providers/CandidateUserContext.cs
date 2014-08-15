namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Common.Providers;

    public class CandidateUserContext : UserContext
    {
        public Guid CandidateId { get; set; }
    }
}
