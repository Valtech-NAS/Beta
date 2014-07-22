namespace SFA.Apprenticeships.Infrastructure.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using NLog;
    using SendGrid;
    using Application.Interfaces.Messaging;
    using Configuration;

    public class SendGridEmailDispatcher : IEmailDispatcher
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly string _userName;
        private readonly string _password;

        private readonly SendGridTemplateConfiguration[] _templates;

        public SendGridEmailDispatcher(SendGridConfiguration configuration)
        {
            _userName = configuration.UserName;
            _password = configuration.Password;
            _templates = configuration.Templates.ToArray();
        }

        public void SendEmail(EmailRequest request)
        {
            Logger.Debug("SendEmail From={0} To={1}, Subject={2}, Template={3}", request.FromEmail, request.ToEmail, request.Subject,request.TemplateName);
            
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
                message.AddSubstitution(
                    DelimitToken(token.Key),
                    new List<string>
                    {
                        token.Value
                    });
            }
        }

        private static string DelimitToken(string key)
        {
            const string templateTokenDelimiter = "-";

            return string.Format("{0}{1}{0}", templateTokenDelimiter, key);
        }

        private void AttachTemplate(EmailRequest request, SendGridMessage message)
        {
            var template = GetTemplateConfiguration(request.TemplateName);
            var fromEmail = DefaultFromEmail(request, template);

            message.From = new MailAddress(fromEmail);
            message.EnableTemplateEngine(template.Id);
        }

        private static string DefaultFromEmail(EmailRequest request, SendGridTemplateConfiguration template)
        {
            return String.IsNullOrWhiteSpace(request.FromEmail) ? template.FromEmail : request.FromEmail;
        }

        private SendGridTemplateConfiguration GetTemplateConfiguration(string templateName)
        {
            var template = _templates
                .FirstOrDefault(each => each.Name == templateName);

            if (template != null) return template;

            var message = string.Format("Invalid email template name: \"{0}\".", templateName);

            Logger.Error("GetTemplateConfiguration : {0}", message);

            // TODO: EXCEPTION: template is invalid, log / throw domain exception.
            throw new Exception(message);
        }

        private void DispatchMessage(SendGridMessage message)
        {
            try
            {
                var credentials = new NetworkCredential(_userName, _password);
                var web = new Web(credentials);

                Logger.Info("Dispatching email to {0}", message.To.ToString());
                web.Deliver(message);
                Logger.Info("Successfully dispatched email to {0}", message.To.ToString());
            }
            catch (Exception e)
            {
                Logger.ErrorException("Failed to dispatch email.", e);
                // TODO: EXCEPTION: failed to send, log / throw domain exception.
                throw new Exception("Failed to dispatch email.", e);
            }
        }
    }
}
