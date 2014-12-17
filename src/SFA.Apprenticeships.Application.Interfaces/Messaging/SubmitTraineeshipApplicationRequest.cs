namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;

    public class SubmitTraineeshipApplicationRequest : BaseRequest
    {
        public Guid ApplicationId { get; set; }
    }
}
