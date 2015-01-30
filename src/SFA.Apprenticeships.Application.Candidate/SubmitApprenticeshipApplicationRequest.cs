namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using Interfaces.Messaging;

    public class SubmitApprenticeshipApplicationRequest : BaseRequest
    {
        public Guid ApplicationId { get; set; }
    }
}
