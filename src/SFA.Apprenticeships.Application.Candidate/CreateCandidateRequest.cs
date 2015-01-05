namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using Interfaces.Messaging;

    public class CreateCandidateRequest : BaseRequest
    {
        public Guid CandidateId { get; set; }
    }
}
