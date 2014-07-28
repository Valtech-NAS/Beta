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
        }

        private IConfigurationManager _configManager;

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

        private IEnumerable<KeyValuePair<CommunicationTokens, string>> CreateTokens()
        {
            return new[]
            {
                new KeyValuePair<CommunicationTokens, string>(
                    CommunicationTokens.ActivationCode, TestActivationCode),
                new KeyValuePair<CommunicationTokens, string>(
                    CommunicationTokens.Username, TestToEmail)
            };
        }

        [Test]
        public void ShoudConstructSendGridEmailDispatcher()
        {
            // Arrange / Act.
            var dispatcher = ObjectFactory.GetInstance<IEmailDispatcher>();

            // Assert.
            Assert.IsNotNull(dispatcher);
            Assert.IsInstanceOf<SendGridEmailDispatcher>(dispatcher);
        }

        [Test]
        public void ShoudSendEmail()
        {
            // Arrange.
            var dispatcher = ObjectFactory.GetInstance<IEmailDispatcher>();

            var request = new EmailRequest
            {
                Subject = "Hello, World at " + DateTime.Now.ToLongTimeString(),
                FromEmail = TestFromEmail,
                ToEmail = TestToEmail,
                Tokens = CreateTokens(),
                MessageType = CandidateMessageTypes.SendActivationCode
            };

            // Act.
            dispatcher.SendEmail(request);

            // Assert: we do not expect an exception.
        }

        [Test]
        public void ShoudSendEmailWithFromEmailInTemplateConfiguration()
        {
            // Arrange.
            var dispatcher = ObjectFactory.GetInstance<IEmailDispatcher>();

            // NOTE: FromEmail is not set and is defined in SendGrid email template.
            var request = new EmailRequest
            {
                Subject = "Hello, World at " + DateTime.Now.ToLongTimeString(),
                ToEmail = TestToEmail,
                Tokens = CreateTokens(),
                MessageType = CandidateMessageTypes.SendActivationCode
            };

            // Act.
            dispatcher.SendEmail(request);

            // Assert: we do not expect an exception.
        }

        [Test]
        public void ShoudSendEmailWithSubjectInTemplate()
        {
            // Arrange.
            var dispatcher = ObjectFactory.GetInstance<IEmailDispatcher>();

            // NOTE: Subject is not set and is defined in SendGrid email template.
            var request = new EmailRequest
            {
                FromEmail = TestFromEmail,
                ToEmail = TestToEmail,
                Tokens = CreateTokens(),
                MessageType = CandidateMessageTypes.SendActivationCode
            };

            // Act.
            dispatcher.SendEmail(request);

            // Assert: we do not expect an exception.
        }
    }
}