namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;

    public class SmsSendMobileVerificationCodeFormatter : SmsMessageFormatter
    {
        public SmsSendMobileVerificationCodeFormatter(TwilioConfiguration configuration) : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.SendMobileVerificationCode").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.MobileVerificationCode).Value);
        }
    }
}