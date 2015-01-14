namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;

    public abstract class BaseRequest
    {
        public DateTime? ProcessTime { get; set; }
    }
}
