namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using SendGrid;

    public class EmailDailyDigestMessageFormatter : EmailMessageFormatter
    {
        private const string Pipe = "|";
        private const char Tilda = '~';

        public const string OneSavedApplicationAboutToExpire = "You've saved an application for an apprenticeship that is due to expire soon.";
        public const string MoreThanOneSaveApplicationAboutToExpire = "You've saved applications for apprenticeships that are due to close soon.";

        public override void PopulateMessage(EmailRequest request, ISendGrid message)
        {
            PopulateItemCountData(request, message);

            PopulateHtmlData(request, message);
        }

        private static void PopulateItemCountData(EmailRequest request, ISendGrid message)
        {
            var itemCountToken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.ExpiringDraftsCount);

            var itemCount = Convert.ToInt32(request.Tokens.First(t => t.Key == CommunicationTokens.ExpiringDraftsCount).Value);

            var substitutionText = itemCount == 1 ? OneSavedApplicationAboutToExpire : MoreThanOneSaveApplicationAboutToExpire;

            AddSubstitutionTo(message, itemCountToken, substitutionText);
        }

        private static void PopulateHtmlData(EmailRequest request, ISendGrid message)
        {
            var sendgridtoken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.ExpiringDrafts);
            AddVacanciesDataSubstitution(request, message, sendgridtoken);
        }

        private static void AddVacanciesDataSubstitution(EmailRequest request, ISendGrid message, string sendgridtoken)
        {
            var substitutionText = "<ul>";

            var drafts = request.Tokens.First(t => t.Key == CommunicationTokens.ExpiringDrafts).Value;

            foreach (var draft in drafts.Split(Tilda))
            {
                substitutionText += FormatHtmlListElement(draft);
            }

            substitutionText += "</ul>";

            AddSubstitutionTo(message, sendgridtoken, substitutionText);
        }

        private static void AddSubstitutionTo(ISendGrid message, string sendgridtoken, string substitutionText)
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