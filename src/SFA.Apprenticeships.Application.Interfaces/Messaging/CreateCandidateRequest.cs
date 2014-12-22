namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;

    //todo: shouldn't be in this project - this is a process specific message so should be in the application layer
    public class CreateCandidateRequest : BaseRequest
    {
        public Guid CandidateId { get; set; }
    }
}
