namespace SFA.Apprenticeships.Infrastructure.Communication
{
    using System;
    using Application.Interfaces.Messaging;

    public class TwilioSmsDispatcher : ISmsDispatcher
    {
        public void SendSms(SmsRequest request)
        {
            // see http://www.strathweb.com/2012/10/send-text-messages-sms-from-web-api-using-azure-mobile-services/

            throw new NotImplementedException();
        }
    }
}
