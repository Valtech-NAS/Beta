namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System;
    using Application.Interfaces.Messaging;

    public class VoidSmsDispatcher : ISmsDispatcher
    {
        public void SendSms(SmsRequest request)
        {
            // We don't want to send anything
        }
    }
}
