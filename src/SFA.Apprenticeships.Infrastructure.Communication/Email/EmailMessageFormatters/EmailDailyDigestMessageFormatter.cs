namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
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
            PopulateCandidateName(request, message);

            PopulateItemCountData(request, message);

            PopulateHtmlData(request, message);
        }

        private static void PopulateCandidateName(EmailRequest request, ISendGrid message)
        {
            var candidateFirstNameToken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.CandidateFirstName);

            var substitutionText = request.Tokens.First(t => t.Key == CommunicationTokens.CandidateFirstName).Value;

            AddSubstitutionTo(message, candidateFirstNameToken, substitutionText);
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
            int vacancyId = 0;
            ExtractVacancyDataFrom(line,out vacancyId, out apprenticeshipName, out companyName, out closingDate);

            return string.Format("<li><a href=\"https://www.findapprenticeship.service.gov.uk/account/apprenticeshipvacancydetails/{0}\">{1} with {2}</a><br>Closing date: {3}</li>",vacancyId, apprenticeshipName, companyName,
                closingDate);
        }

        private static void ExtractVacancyDataFrom(string line, out int vacancyId, out string apprenticeshipName, out string companyName, out string closingDate)
        {
            vacancyId = Convert.ToInt32(line.Split(new[] { Pipe }, StringSplitOptions.RemoveEmptyEntries)[0]);
            apprenticeshipName = WebUtility.UrlDecode(line.Split(new[] {Pipe}, StringSplitOptions.RemoveEmptyEntries)[1]);
            companyName = WebUtility.UrlDecode(line.Split(new[] {Pipe}, StringSplitOptions.RemoveEmptyEntries)[2]);
            closingDate = line.Split(new[] {Pipe}, StringSplitOptions.RemoveEmptyEntries)[3];
        }
    }
}