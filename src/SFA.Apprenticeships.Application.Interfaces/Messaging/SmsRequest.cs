namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;

    /// <summary>
    /// DTO to represent an SMS that should be sent
    /// </summary>
    public class SmsRequest
    {
        public string Sender { get; set; }
        public string MobileNumber { get; set; }
        public string Message { get; set; }
    }
}
