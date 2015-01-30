namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using SendGrid;

    public class EmailDailyDigestMessageFormatter : EmailMessageFormatter
    {
        private const string OneSavedApplicationAboutToExpire = "You've saved an application for an apprenticeship that is due to expire soon.";

        private const string MoreThanOneSaveApplicationAboutToExpire =
            "You've saved applications for apprenticeships that are due to close soon.";
        private const string Pipe = "|";

        public override void PopulateMessage(EmailRequest request, SendGridMessage message)
        {
            var itemCount = PopulateItemCountData(request, message);

            PopulateHtmlData(request, message, itemCount);
        }

        private static int PopulateItemCountData(EmailRequest request, SendGridMessage message)
        {
            var itemCountToken =
                SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.TotalItems);

            var itemCount = Convert.ToInt32(request.Tokens.First(t => t.Key == CommunicationTokens.TotalItems).Value);

            var substitutionText = itemCount == 1 ? OneSavedApplicationAboutToExpire : MoreThanOneSaveApplicationAboutToExpire;

            AddSubstitutionTo(message, itemCountToken, substitutionText);

            return Convert.ToInt32(itemCount);
        }

        private static void PopulateHtmlData(EmailRequest request, SendGridMessage message, int itemCount)
        {
            var sendgridtoken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.Item1);
            AddVacanciesDataSubstitution(request, message, itemCount, sendgridtoken);
        }

        private static void AddVacanciesDataSubstitution(EmailRequest request, SendGridMessage message, int itemCount, string sendgridtoken )
        {
            var substitutionText = "<ul>";

            for (var i = 0; i < itemCount; i++)
            {
                var communicationToken = (CommunicationTokens) Enum.Parse(typeof (CommunicationTokens),
                    string.Format("Item{0}", i+1));

                substitutionText += FormatHtmlListElement(request.Tokens.First(t => t.Key == communicationToken).Value);
            }

            substitutionText += "</ul>";

            AddSubstitutionTo(message, sendgridtoken, substitutionText);
        }

        private static void AddSubstitutionTo(SendGridMessage message, string sendgridtoken, string substitutionText)
        {
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

            return string.Format("<li>{0} with {1}<br>Closing date: {2}</li>", apprenticeshipName, companyName,
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