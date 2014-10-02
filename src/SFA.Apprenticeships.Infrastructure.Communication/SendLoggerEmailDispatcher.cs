namespace SFA.Apprenticeships.Infrastructure.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using Application.Interfaces.Messaging;
    using Configuration;
    using SendGrid;

    public class SendLoggerEmailDispatcher : ILoggerEmailDispatcher
    {
        private readonly string _password;
        private readonly string _userName;

        private readonly SendGridTemplateConfiguration[] _templates;
        private static string _logToEmail;

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
            var message = CreateMessage(request);

            AttachTemplate(request, message);
            PopulateTemplate(request, message);

            return message;
        }

        private static SendGridMessage CreateMessage(LogRequest request)
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
            var templateName = GetTemplateName(request.MessageType);
            var template = GetTemplateConfiguration(templateName);
            var fromEmail = DefaultFromEmail(template);

            message.From = new MailAddress(fromEmail);
            message.EnableTemplateEngine(template.Id);
        }

        private static string GetTemplateName(Enum messageType)
        {
            var enumType = messageType.GetType();
            var templateName = string.Format("{0}.{1}", enumType.Name, Enum.GetName(enumType, messageType));
           
            return templateName;
        }

        private static string DefaultFromEmail(SendGridTemplateConfiguration template)
        {
            return template.FromEmail;
        }

        private SendGridTemplateConfiguration GetTemplateConfiguration(string templateName)
        {
            return _templates.FirstOrDefault(each => each.Name == templateName);
        }

        private void DispatchMessage(SendGridMessage message)
        {
            try
            {
                var credentials = new NetworkCredential(_userName, _password);
                var web = new Web(credentials);
              
                web.Deliver(message);               
            }
            catch (Exception e)
            {
                
            }
        }      
    }
}