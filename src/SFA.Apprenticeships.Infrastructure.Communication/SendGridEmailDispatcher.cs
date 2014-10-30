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
    using Domain.Entities.Exceptions;
    using NLog;
    using SendGrid;

    public class SendGridEmailDispatcher : IEmailDispatcher
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly string _password;

        private readonly SendGridTemplateConfiguration[] _templates;
        private readonly string _userName;

        public SendGridEmailDispatcher(SendGridConfiguration configuration)
        {
            _userName = configuration.UserName;
            _password = configuration.Password;
            _templates = configuration.Templates.ToArray();
        }

        public void SendEmail(EmailRequest request)
        {
            var message = ComposeMessage(request);
            DispatchMessage(message);
        }

        private SendGridMessage ComposeMessage(EmailRequest request)
        {
            var message = CreateMessage(request);

            AttachTemplate(request, message);
            PopulateTemplate(request, message);

            return message;
        }

        private static SendGridMessage CreateMessage(EmailRequest request)
        {
            const string emptyHtml = "<span></span>";
            const string emptyText = "";

            var subject = DefaultSubject(request);

            // NOTE: https://github.com/sendgrid/sendgrid-csharp.
            var message = new SendGridMessage
            {
                Subject = subject,
                To = new[]
                {
                    new MailAddress(request.ToEmail)
                },
                Text = emptyText,
                Html = emptyHtml
            };

            return message;
        }

        private static string DefaultSubject(EmailRequest request)
        {
            const string emptySubject = " "; // CRITICAL: must be a single space.

            return string.IsNullOrWhiteSpace(request.Subject) ? emptySubject : request.Subject;
        }

        private static void PopulateTemplate(EmailRequest request, SendGridMessage message)
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

        private void AttachTemplate(EmailRequest request, SendGridMessage message)
        {
            var templateName = GetTemplateName(request.MessageType);
            var template = GetTemplateConfiguration(templateName);
            var fromEmail = DefaultFromEmail(request, template);

            message.From = new MailAddress(fromEmail);
            message.EnableTemplateEngine(template.Id);
        }

        private static string GetTemplateName(Enum messageType)
        {
            var enumType = messageType.GetType();
            var templateName = string.Format("{0}.{1}", enumType.Name, Enum.GetName(enumType, messageType));
            Logger.Debug("Determined email template: EnumType={0} Name={1} TemplateName={2} MessageType={3}", enumType, enumType.Name, templateName, messageType);
            return templateName;
        }

        private static string DefaultFromEmail(EmailRequest request, SendGridTemplateConfiguration template)
        {
            return String.IsNullOrWhiteSpace(request.FromEmail) ? template.FromEmail : request.FromEmail;
        }

        private SendGridTemplateConfiguration GetTemplateConfiguration(string templateName)
        {
            var template = _templates.FirstOrDefault(each => each.Name == templateName);

            if (template != null)
            {
                return template;
            }

            var errorMessage = string.Format("GetTemplateConfiguration : Invalid email template name: {0}",
                templateName);
            Logger.Error(errorMessage);

            throw new ConfigurationErrorsException(errorMessage);
        }

        private void DispatchMessage(SendGridMessage message)
        {
            try
            {
                var credentials = new NetworkCredential(_userName, _password);
                var web = new Web(credentials);

                Logger.Debug("Dispatching email: {0}", LogSendGridMessage(message));
                web.Deliver(message);
                Logger.Info("Dispatched email: {0} to {1}", message.Subject, message.To);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to dispatch email", e);
                throw new CustomException("Failed to dispatch email", e, ErrorCodes.EmailSendGridError);
            }
        }

        private static string LogSendGridMessage(SendGridMessage message)
        {
            var messageLog = string.Format("Subject: {0}", message.Subject);
            messageLog += "To: ";
            message.To.ToList().ForEach(t => messageLog += string.Format("{0}, ", t.Address));
            messageLog += string.Format("From: {0}, ", message.From.Address);

            return messageLog;
        }
    }
}
