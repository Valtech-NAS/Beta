namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using Application.Interfaces.Communications;

    public class VoidSmsDispatcher : ISmsDispatcher
    {
        public void SendSms(SmsRequest request)
        {
            // We don't want to send anything
        }
    }
}
