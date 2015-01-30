namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Messaging;

    public class SmsDailyDigestMessageFormatter : SmsMessageFormatter
    {
        public SmsDailyDigestMessageFormatter(TwilioConfiguration configuration) : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.DailyDigest").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            var itemCount = communicationTokens.First(t => t.Key == CommunicationTokens.TotalItems).Value;

            return string.Format(Message, itemCount);
        }
    }
}