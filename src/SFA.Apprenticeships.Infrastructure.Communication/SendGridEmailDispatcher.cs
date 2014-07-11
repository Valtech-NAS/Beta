using System;
using System.Collections.Generic;

namespace SFA.Apprenticeships.Infrastructure.Communication
{
    using System.Net;
    using System.Net.Mail;
    using SendGrid;
    using Application.Interfaces.Messaging;
    using Common.Configuration;
    using Common.Rest;

    public class SendGridEmailDispatcher : IEmailDispatcher
    {
        private readonly string _userName;
        private readonly string _password;

        public SendGridEmailDispatcher(IConfigurationManager configurationManager)
        {
            _userName = configurationManager.GetAppSetting("SendGridUserName");
            _password = configurationManager.GetAppSetting("SendGridPassword");
        }

        public void SendEmail(EmailRequest request)
        {
            var message = new SendGridMessage
            {
                Subject = request.Subject,
                From = new MailAddress(request.FromEmail),
                To = new[]
                {
                    new MailAddress(request.ToEmail)
                },
                Text = "Hello.",
                Html = "<strong>Hello from HTML.</strong>"
            };

            // message.EnableTemplateEngine("Candidate-Registration-Activation-Code");

            foreach (var token in request.Tokens)
            {
                message.AddSubstitution(token.Key, new List<string> { token.Value });
            }

            var credentials = new NetworkCredential(_userName, _password);
            var web = new Web(credentials);

            try
            {
                web.Deliver(message);
            }
            catch (Exception)
            {
                throw;
            }

            //todo: SendGridEmailDispatcher... read sendgrid account details from config, use REST API to invoke
            //note: https://github.com/sendgrid/sendgrid-csharp
            //note: https://sendgrid.com/docs/API_Reference/SMTP_API/substitution_tags.html
        }
    }
}
