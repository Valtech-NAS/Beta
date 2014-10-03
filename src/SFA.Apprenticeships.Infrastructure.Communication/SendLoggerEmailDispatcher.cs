namespace SFA.Apprenticeships.Infrastructure.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using Application.Interfaces.Messaging;
    using Configuration;
    using SendGrid;

    public class SendLoggerEmailDispatcher : ILoggerEmailDispatcher
    {
        private static string _logToEmail;
        private readonly string _password;

        private readonly SendGridTemplateConfiguration[] _templates;
        private readonly string _userName;

        public SendLoggerEmailDispatcher(SendGridConfiguration configuration)
        {
            _userName = configuration.UserName;
            _password = configuration.Password;
            _logToEmail = configuration.LogToEmailAddress;
            _templates = configuration.Templates.ToArray();
        }

        public void SendLogViaEmail(LogRequest request)
        {
            var message = ComposeMessage(request);
            DispatchMessage(message);
        }

        private SendGridMessage ComposeMessage(LogRequest request)
        {
            var message = CreateMessage();

            AttachTemplate(request, message);
            PopulateTemplate(request, message);

            return message;
        }

        private static SendGridMessage CreateMessage()
        {
            const string emptyHtml = "<span></span>";
            const string emptyText = "";

            // NOTE: https://github.com/sendgrid/sendgrid-csharp.
            var message = new SendGridMessage
            {
                Subject = " Fallback Notification ",
                To = new[]
                {
                    new MailAddress(_logToEmail)
                },
                Text = emptyText,
                Html = emptyHtml
            };

            return message;
        }


        private static void PopulateTemplate(LogRequest request, SendGridMessage message)
        {
            // NOTE: https://sendgrid.com/docs/API_Reference/SMTP_API/substitution_tags.html.
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

        private void AttachTemplate(LogRequest request, SendGridMessage message)
        {
            var enumType = request.MessageType.GetType();
            var templateName = string.Format("{0}.{1}", enumType.Name, Enum.GetName(enumType, request.MessageType));
            var template = _templates.FirstOrDefault(each => each.Name == templateName);
            if (template == null)
            {
                return;
            }

            var fromEmail = template.FromEmail;
            message.From = new MailAddress(fromEmail);
            message.EnableTemplateEngine(template.Id);
        }

        private void DispatchMessage(SendGridMessage message)
        {
            var credentials = new NetworkCredential(_userName, _password);
            var web = new Web(credentials);

            web.Deliver(message);
        }
    }
}