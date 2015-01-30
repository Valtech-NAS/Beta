namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Messaging;

    public class SmsAccountUnlockCodeMessageFormatter : SmsMessageFormatter
    {
        public SmsAccountUnlockCodeMessageFormatter(TwilioConfiguration configuration)
            : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.SendAccountUnlockCode").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.AccountUnlockCode).Value);
        }
    }
}