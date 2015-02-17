namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;

    public class SmsActivationCodeMessageFormatter : SmsMessageFormatter
    {
        public SmsActivationCodeMessageFormatter(ITwillioConfiguration configuration)
            : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.SendActivationCode").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return string.Format(Message,
                communicationTokens.First(ct => ct.Key == CommunicationTokens.ActivationCode).Value);
        }
    }
}