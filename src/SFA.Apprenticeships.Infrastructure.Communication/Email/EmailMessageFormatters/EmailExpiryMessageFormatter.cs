namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Messaging;
    using SendGrid;

    public class EmailExpiryMessageFormatter : EmailMessageFormatter
    {
        private const string Pipe = "|";

        public override void PopulateMessage(EmailRequest request, SendGridMessage message)
        {
            var itemCount = PopulateItemCountData(request, message);

            PopulateHtmlData(request, message, itemCount);

            PopulateTextData(request, message, itemCount);
        }

        private static int PopulateItemCountData(EmailRequest request, SendGridMessage message)
        {
            var itemCountToken =
                SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.ItemCount);

            message.AddSubstitution(
                itemCountToken,
                new List<string>
                {
                    request.Tokens.First(t => t.Key == CommunicationTokens.ItemCount).Value
                });

            return Convert.ToInt32(itemCountToken);
        }

        private static void PopulateHtmlData(EmailRequest request, SendGridMessage message, int itemCount)
        {
            var sendgridtoken = SendGridTokenManager.VacancyAboutToExpireVacanciesInfoHtmlToken;
            AddVacanciesDataSubstitution(request, message, itemCount, sendgridtoken, FormatHtmlListElement);
        }

        private static void PopulateTextData(EmailRequest request, SendGridMessage message, int itemCount)
        {
            var sendgridtoken = SendGridTokenManager.VacancyAboutToExpireVacanciesInfoTextToken;
            AddVacanciesDataSubstitution(request, message, itemCount, sendgridtoken, FormatTextListElement);
        }

        private static void AddVacanciesDataSubstitution(EmailRequest request, SendGridMessage message, int itemCount, string sendgridtoken, Func<string, string> formatFunction )
        {
            var substitutionText = string.Empty;

            for (var i = 0; i < itemCount; i++)
            {
                var communicationToken = (CommunicationTokens) Enum.Parse(typeof (CommunicationTokens),
                    string.Format("CommunicationTokens.Item{0}", i));

                substitutionText += formatFunction(request.Tokens.First(t => t.Key == communicationToken).Value);
            }

            message.AddSubstitution(
                sendgridtoken,
                new List<string>
                {
                    substitutionText
                });
        }

        private static string FormatHtmlListElement(string line)
        {
            string apprenticeshipName;
            string companyName;
            string closingDate;
            ExtractVacancyDataFrom(line, out apprenticeshipName, out companyName, out closingDate);

            return string.Format("<li>{0} with {1}</br><b>Closing date:</b> {2}</li>", apprenticeshipName, companyName,
                closingDate);
        }

        private static string FormatTextListElement(string line)
        {
            string apprenticeshipName;
            string companyName;
            string closingDate;
            ExtractVacancyDataFrom(line, out apprenticeshipName, out companyName, out closingDate);

            return string.Format("{0} with {1}.\\nClosing date: {2}\\n\\n", apprenticeshipName, companyName,
                closingDate);
        }

        private static void ExtractVacancyDataFrom(string line, out string apprenticeshipName, out string companyName, out string closingDate)
        {
            apprenticeshipName = line.Split(new[] {Pipe}, StringSplitOptions.RemoveEmptyEntries)[0];
            companyName = line.Split(new[] {Pipe}, StringSplitOptions.RemoveEmptyEntries)[1];
            closingDate = line.Split(new[] {Pipe}, StringSplitOptions.RemoveEmptyEntries)[2];
        }
    }
}