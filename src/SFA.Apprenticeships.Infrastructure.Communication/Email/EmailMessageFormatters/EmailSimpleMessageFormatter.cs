namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters
{
    using System.Collections.Generic;
    using Application.Interfaces.Messaging;
    using SendGrid;

    public class EmailSimpleMessageFormatter : EmailMessageFormatter
    {
        public override void PopulateMessage(EmailRequest request, SendGridMessage message)
        {
            foreach (var token in request.Tokens)
            {
                var sendgridtoken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(token.Key);
                message.AddSubstitution(
                    sendgridtoken,
                    new List<string>
                    {
                        token.Value
                    });
            }
        }
    }
}