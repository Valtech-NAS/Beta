namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Messaging;

    public class SmsApprenticeshipApplicationSubmittedMessageFormatter : SmsMessageFormatter
    {
        public SmsApprenticeshipApplicationSubmittedMessageFormatter(TwilioConfiguration configuration)
            : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.ApprenticeshipApplicationSubmitted").Message;
        }

        public override string GetMessage(IEnumerable<KeyValuePair<CommunicationTokens, string>> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.ApplicationVacancyReference).Value);
        }
    }
}