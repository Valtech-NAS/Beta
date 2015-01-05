namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using Interfaces.Messaging;

    public class SubmitTraineeshipApplicationRequest : BaseRequest
    {
        public Guid ApplicationId { get; set; }
    }
}
