namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using Interfaces.Messaging;

    public class SubmitApplicationRequest : BaseRequest
    {
        public Guid ApplicationId { get; set; }
    }
}
