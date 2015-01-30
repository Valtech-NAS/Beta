namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using Application.Interfaces.Messaging;

    public class SmsPasswordChangedMessageFormatter : SmsMessageFormatter
    {
        public SmsPasswordChangedMessageFormatter(TwilioConfiguration configuration)
            : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.PasswordChanged").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return Message;
        }
    }
}