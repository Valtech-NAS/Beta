namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Application.Interfaces.Communications;
    using Domain.Entities;
    using SendGrid;

    public class EmailDailyDigestMessageFormatter : EmailMessageFormatter
    {
        private const string Pipe = "|";
        private const char Tilda = '~';

        public override void PopulateMessage(EmailRequest request, ISendGrid message)
        {
            PopulateFullName(request, message);
        }

        private static void PopulateFullName(EmailRequest request, ISendGrid message)
        {
            var candidateFirstNameToken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.FullName);

            var substitutionText = request.Tokens.First(t => t.Key == CommunicationTokens.FullName).Value;

            AddSubstitutionTo(message, candidateFirstNameToken, substitutionText);
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
    }
}