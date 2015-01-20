namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Messaging;

    public class SmsActivationCodeMessageFormatter : SmsMessageFormatter
    {
        public SmsActivationCodeMessageFormatter(TwilioConfiguration configuration)
            : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.SendActivationCode").Message;
        }

        public override string GetMessage(IEnumerable<KeyValuePair<CommunicationTokens, string>> communicationTokens)
        {
            return string.Format(Message,
                communicationTokens.First(ct => ct.Key == CommunicationTokens.ActivationCode).Value);
        }
    }
}