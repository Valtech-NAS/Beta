namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;

    /// <summary>
    /// DTO to represent an email that should be sent
    /// </summary>
    public class SmsRequest
    {
        public string MobileNumber { get; set; }
        public string Message { get; set; }
    }
}
