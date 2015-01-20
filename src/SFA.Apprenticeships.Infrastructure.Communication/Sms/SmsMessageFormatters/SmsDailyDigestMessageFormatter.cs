namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using Application.Interfaces.Messaging;

    public class SmsDailyDigestMessageFormatter : SmsMessageFormatter
    {
        public SmsDailyDigestMessageFormatter(TwilioConfiguration configuration) : base(configuration)
        {
            Message = GetTemplateConfiguration("MessageTypes.PasswordChanged").Message;
        }

        public override string GetMessage(IEnumerable<KeyValuePair<CommunicationTokens, string>> communicationTokens)
        {
            // count the number of about to expire vacancies
            var numberOfVacanciesAboutToExpire = 6;

            return string.Format(Message, numberOfVacanciesAboutToExpire);
        }
    }
}