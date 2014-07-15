namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using StructureMap;
    using Common.IoC;
    using IoC;
    using Common.Configuration;
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
                Tokens = new[]
                {
                    new KeyValuePair<string, string>(
                        "Candidate.ActivationCode", DateTime.Now.ToLongDateString())
                },
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
                Tokens = new[]
                {
                    new KeyValuePair<string, string>(
                        "Candidate.ActivationCode", DateTime.Now.ToLongDateString())
                },
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
                Tokens = new[]
                {
                    new KeyValuePair<string, string>(
                        "Candidate.ActivationCode", DateTime.Now.ToLongDateString())
                },
                TemplateName = TestTemplateName
            };

            // Act.
            dispatcher.SendEmail(request);

            // Assert: we do not expect an exception.
        }

        [Test]
        // TODO: AG: make exception more specific when exceptions generally.
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
                Tokens = new KeyValuePair<string, string>[]
                {
                },
                TemplateName = "Invalid.Template.Name"
            };

            // Act.
            dispatcher.SendEmail(request);
        }

        public string TestToEmail { get { return _configManager.GetAppSetting("Email.Test.To"); } }

        public string TestFromEmail { get { return _configManager.GetAppSetting("Email.Test.From"); } }

        public string TestTemplateName { get { return _configManager.GetAppSetting("Email.Test.TemplateName"); } }
    }
}
