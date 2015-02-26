namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters
{
    using System.Collections.Generic;
    using Application.Interfaces.Communications;
    using Domain.Entities;
    using SendGrid;

    public class EmailSimpleMessageFormatter : EmailMessageFormatter
    {
        public override void PopulateMessage(EmailRequest request, ISendGrid message)
        {
            foreach (var token in request.Tokens)
            {
                var sendgridtoken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(token.Key);
                message.AddSubstitution(sendgridtoken,
                    new List<string>
                    {
                        token.Value
                    });
            }
        }
    }
}
