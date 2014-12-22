namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Messaging;
    using Common.IoC;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class SendGridEmailDispatcherTests
    {
        [SetUp]
        public void SetUp()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommunicationRegistry>();
                x.AddRegistry<CommonRegistry>();
            });

            _dispatcher = ObjectFactory.GetNamedInstance<IEmailDispatcher>("SendGridEmailDispatcher");
            _voidEmailDispatcher = ObjectFactory.GetNamedInstance<IEmailDispatcher>("VoidEmailDispatcher");
#pragma warning restore 0618
        }

        private IEmailDispatcher _dispatcher;
        private IEmailDispatcher _voidEmailDispatcher;

        private const string TestToEmail = "valtechnas@gmail.com";

        private const string TestActivationCode = "ABC123"; 

        private const string TestFromEmail = "from@example.com";
        
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

        private static IEnumerable<KeyValuePair<CommunicationTokens, string>> CreateApprenticeshipApplicationSubmittedTokens()
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

        private static IEnumerable<KeyValuePair<CommunicationTokens, string>> CreateTraineeshipApplicationSubmittedTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, "FirstName"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyTitle,
                    "Application Vacancy Title"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ApplicationVacancyReference,
                    "Application Vacancy Reference"),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ProviderContact,
                    "Provider Contact")
            };
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShoudConstructSendGridEmailDispatcher()
        {
            Assert.IsNotNull(_dispatcher);
            Assert.IsInstanceOf<SendGridEmailDispatcher>(_dispatcher);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldConstructVoidEmailDispatcher()
        {
            Assert.IsNotNull(_voidEmailDispatcher);
            Assert.IsInstanceOf<VoidEmailDispatcher>(_voidEmailDispatcher);
        }

        [Test, Category("Integration")]
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

        [Test, Category("Integration"), Category("SmokeTests")]
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

        [Test, Category("Integration")]
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

        [Test, Category("Integration")]
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

        [Test, Category("Integration")]
        public void ShouldSendApprenticeshipApplicationSubmittedEmail()
        {
            var request = new EmailRequest
            {
                FromEmail = TestFromEmail,
                ToEmail = TestToEmail,
                Tokens = CreateApprenticeshipApplicationSubmittedTokens(),
                MessageType = CandidateMessageTypes.ApprenticeshipApplicationSubmitted
            };

            _dispatcher.SendEmail(request);
        }

        [Test, Category("Integration")]
        public void ShouldSendTraineeshipApplicationSubmittedEmail()
        {
            var request = new EmailRequest
            {
                FromEmail = TestFromEmail,
                ToEmail = TestToEmail,
                Tokens = CreateTraineeshipApplicationSubmittedTokens(),
                MessageType = CandidateMessageTypes.TraineeshipApplicationSubmitted
            };

            _dispatcher.SendEmail(request);
        }

        [Test, Category("Integration")]
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

        [Test, Category("Integration")]
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