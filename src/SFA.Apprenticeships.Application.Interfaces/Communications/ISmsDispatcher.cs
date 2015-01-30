namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System;

    /// <summary>
    /// Used to queue / send an SMS message
    /// </summary>
    public interface ISmsDispatcher
    {
        void SendSms(SmsRequest request);
    }
}
