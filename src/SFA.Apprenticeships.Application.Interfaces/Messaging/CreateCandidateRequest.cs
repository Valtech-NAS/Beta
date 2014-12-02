namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;

    public class CreateCandidateRequest : BaseRequest
    {
        public Guid CandidateId { get; set; }
    }
}
