namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Messaging;
    using Common.IoC;
    using Domain.Interfaces.Configuration;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class SendGridEmailDispatcherTests
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommunicationRegistry>();
                x.AddRegistry<CommonRegistry>();
            });

            _configManager = ObjectFactory.GetInstance<IConfigurationManager>();
            _dispatcher = ObjectFactory.GetInstance<IEmailDispatcher>();
        }

        private IConfigurationManager _configManager;
        private IEmailDispatcher _dispatcher;

        private string TestToEmail
        {
            get { return _configManager.GetAppSetting("Email.Test.To"); }
        }

        private string TestActivationCode
        {
            get { return "ABC123"; }
        }

        private string TestFromEmail
        {
            get { return _configManager.GetAppSetting("Email.Test.From"); }
        }

        private IEnumerable<KeyValuePair<CommunicationTokens, string>> CreateActivationEmailTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, "FirstName"),
                new KeyValuePair<CommunicationTokens, string>(
                    CommunicationTokens.ActivationCode, TestActivationCode),
                new KeyValuePair<CommunicationTokens, string>(
                    CommunicationTokens.Username, TestToEmail),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ActivationCodeExpiryDays,
                    " 30 days")
            };
        }

        private IEnumerable<KeyValuePair<CommunicationTokens, string>> CreateAccountUnlockCodeTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, "FirstName"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.Username, TestToEmail),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.AccountUnlockCode, TestActivationCode),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.AccountUnlockCodeExpiryDays,
                    " 1 day")
            };
        }

        private IEnumerable<KeyValuePair<CommunicationTokens, string>> CreatePasswordResetConfirmationTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, "FirstName"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.Username, TestToEmail)
            };
        }

        private IEnumerable<KeyValuePair<CommunicationTokens, string>> CreatePasswordResetTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, "FirstName"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.Username, TestToEmail),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.PasswordResetCode, TestActivationCode),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.PasswordResetCodeExpiryDays, "1 day")
            };
        }

        private static IEnumerable<KeyValuePair<CommunicationTokens, string>> CreateApplicationSubmittedTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, "FirstName"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyTitle,
                    "Application Vacancy Title"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyReference,
                    "Application Vacancy Reference")
            };
        }

        [Test]
        public void ShoudConstructSendGridEmailDispatcher()
        {
            Assert.IsNotNull(_dispatcher);
            Assert.IsInstanceOf<SendGridEmailDispatcher>(_dispatcher);
        }

        [Test]
        public void ShoudSendEmail()
        {
            var request = new EmailRequest
            {
                Subject = "Hello, World at " + DateTime.Now.ToLongTimeString(),
                FromEmail = TestFromEmail,
                ToEmail = TestToEmail,
                Tokens = CreateActivationEmailTokens(),
                MessageType = CandidateMessageTypes.SendActivationCode
            };

            _dispatcher.SendEmail(request);
        }

        [Test]
        public void ShoudSendEmailWithFromEmailInTemplateConfiguration()
        {
            // NOTE: FromEmail is not set and is defined in SendGrid email template.
            var request = new EmailRequest
            {
                Subject = "Hello, World at " + DateTime.Now.ToLongTimeString(),
                ToEmail = TestToEmail,
                Tokens = CreateActivationEmailTokens(),
                MessageType = CandidateMessageTypes.SendActivationCode
            };

            _dispatcher.SendEmail(request);
        }

        [Test]
        public void ShoudSendEmailWithSubjectInTemplate()
        {
            // NOTE: Subject is not set and is defined in SendGrid email template.
            var request = new EmailRequest
            {
                FromEmail = TestFromEmail,
                ToEmail = TestToEmail,
                Tokens = CreateActivationEmailTokens(),
                MessageType = CandidateMessageTypes.SendActivationCode
            };

            _dispatcher.SendEmail(request);
        }

        [Test]
        public void ShouldSendAccountUnlockCode()
        {
            var request = new EmailRequest
            {
                FromEmail = TestFromEmail,
                ToEmail = TestToEmail,
                Tokens = CreateAccountUnlockCodeTokens(),
                MessageType = CandidateMessageTypes.SendAccountUnlockCode
            };

            _dispatcher.SendEmail(request);
        }

        [Test]
        public void ShouldSendApplicationSubmittedEmail()
        {
            var request = new EmailRequest
            {
                FromEmail = TestFromEmail,
                ToEmail = TestToEmail,
                Tokens = CreateApplicationSubmittedTokens(),
                MessageType = CandidateMessageTypes.ApplicationSubmitted
            };

            _dispatcher.SendEmail(request);
        }

        [Test]
        public void ShouldSendPasswordResetCodeEmail()
        {
            var request = new EmailRequest
            {
                FromEmail = TestFromEmail,
                ToEmail = TestToEmail,
                Tokens = CreatePasswordResetTokens(),
                MessageType = CandidateMessageTypes.SendPasswordResetCode
            };

            _dispatcher.SendEmail(request);
        }

        [Test]
        public void ShouldSendPasswordResetConfirmationEmail()
        {
            var request = new EmailRequest
            {
                FromEmail = TestFromEmail,
                ToEmail = TestToEmail,
                Tokens = CreatePasswordResetConfirmationTokens(),
                MessageType = CandidateMessageTypes.PasswordChanged
            };

            _dispatcher.SendEmail(request);
        }
    }
}