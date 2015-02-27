namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;

    public class SmsApprenticeshipApplicationSubmittedMessageFormatter : SmsMessageFormatter
    {
        public SmsApprenticeshipApplicationSubmittedMessageFormatter(ITwillioConfiguration configuration)
            : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.ApprenticeshipApplicationSubmitted").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            var commTokens = communicationTokens as IList<CommunicationToken> ?? communicationTokens.ToList();
            var vacancyTitle = commTokens.First(ct => ct.Key == CommunicationTokens.ApplicationVacancyTitle).Value;
            var employerName = commTokens.First(ct => ct.Key == CommunicationTokens.ApplicationVacancyEmployerName).Value;
            return string.Format(Message, vacancyTitle, employerName);
        }
    }
}