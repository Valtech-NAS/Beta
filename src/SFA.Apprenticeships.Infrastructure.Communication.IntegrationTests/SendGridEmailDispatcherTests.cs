namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using Domain.Interfaces.Configuration;
    using NUnit.Framework;
    using StructureMap;
    using Common.IoC;
    using IoC;
    using Application.Interfaces.Messaging;

    [TestFixture]
    public class SendGridEmailDispatcherTests
    {
        private IConfigurationManager _configManager;

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
                TemplateName = TestTemplateName
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
                TemplateName = TestTemplateName
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
                TemplateName = TestTemplateName
            };

            // Act.
            dispatcher.SendEmail(request);

            // Assert: we do not expect an exception.
        }

        [Test]
        // TODO: EXCEPTION: make exception more specific when exceptions generally.
        [ExpectedException(typeof(Exception))]
        public void ShouldThrowIfTemplateNameIsInvalid()
        {
            // Arrange.
            var dispatcher = ObjectFactory.GetInstance<IEmailDispatcher>();

            var request = new EmailRequest
            {
                Subject = "Hello, World at " + DateTime.Now.ToLongTimeString(),
                FromEmail = TestFromEmail,
                ToEmail = TestToEmail,
                Tokens = CreateTokens(),
                TemplateName = "Invalid.Template.Name"
            };

            // Act.
            dispatcher.SendEmail(request);
        }

        private string TestToEmail { get { return _configManager.GetAppSetting("Email.Test.To"); } }

        private string TestActivationCode { get { return "ABC123"; } }

        private string TestFromEmail { get { return _configManager.GetAppSetting("Email.Test.From"); } }

        private string TestTemplateName { get { return _configManager.GetAppSetting("Email.Test.TemplateName"); } }

        private IEnumerable<KeyValuePair<string, string>> CreateTokens()
        {
            return new[]
            {
                new KeyValuePair<string, string>(
                    "Candidate.ActivationCode", TestActivationCode),
                new KeyValuePair<string, string>(
                    "Candidate.EmailAddress", TestToEmail),
            };
        }
    }
}
