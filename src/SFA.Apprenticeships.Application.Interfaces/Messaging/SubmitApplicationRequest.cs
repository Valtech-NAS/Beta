namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;

    public class SubmitApplicationRequest : BaseRequest
    {
        public Guid ApplicationId { get; set; }
    }
}
