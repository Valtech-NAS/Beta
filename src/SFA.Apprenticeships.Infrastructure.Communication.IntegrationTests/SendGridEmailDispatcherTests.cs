namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using System;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Logging;
    using Common.IoC;
    using Email;
    using IoC;
    using Moq;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class SendGridEmailDispatcherTests
    {
        private IEmailDispatcher _dispatcher;

        private IEmailDispatcher _voidEmailDispatcher;

        private readonly Mock<ILogService> _logServiceMock = new Mock<ILogService>();

        private const string TestToEmail = "valtechnas@gmail.com";

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<CommunicationRegistry>();
            });

            container.Configure(c => c.For<ILogService>().Use(_logServiceMock.Object));

            _dispatcher = container.GetInstance<IEmailDispatcher>("SendGridEmailDispatcher");
            _voidEmailDispatcher = container.GetInstance<IEmailDispatcher>("VoidEmailDispatcher");
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
                ToEmail = TestToEmail,
                Tokens = TokenGenerator.CreateActivationEmailTokens(),
                MessageType = MessageTypes.SendActivationCode
            };

            _dispatcher.SendEmail(request);

            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShoudSendEmailWithFromEmailInTemplateConfiguration()
        {
            // NOTE: FromEmail is not set and is defined in SendGrid email template.
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = TokenGenerator.CreateActivationEmailTokens(),
                MessageType = MessageTypes.SendActivationCode
            };

            _dispatcher.SendEmail(request);

            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }

        [Test, Category("Integration")]
        public void ShoudSendEmailWithSubjectInTemplate()
        {
            // NOTE: Subject is not set and is defined in SendGrid email template.
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = TokenGenerator.CreateActivationEmailTokens(),
                MessageType = MessageTypes.SendActivationCode
            };

            _dispatcher.SendEmail(request);

            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }

        [Test, Category("Integration")]
        public void ShouldSendAccountUnlockCode()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = TokenGenerator.CreateAccountUnlockCodeTokens(),
                MessageType = MessageTypes.SendAccountUnlockCode
            };

            _dispatcher.SendEmail(request);

            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }

        [Test, Category("Integration")]
        public void ShouldSendApprenticeshipApplicationSubmittedEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = TokenGenerator.CreateApprenticeshipApplicationSubmittedTokens(),
                MessageType = MessageTypes.ApprenticeshipApplicationSubmitted
            };

            _dispatcher.SendEmail(request);

            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }

        [Test, Category("Integration")]
        public void ShouldSendTraineeshipApplicationSubmittedEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = TokenGenerator.CreateTraineeshipApplicationSubmittedTokens(),
                MessageType = MessageTypes.TraineeshipApplicationSubmitted
            };

            _dispatcher.SendEmail(request);

            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }

        [Test, Category("Integration")]
        public void ShouldSendPasswordResetCodeEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = TokenGenerator.CreatePasswordResetTokens(),
                MessageType = MessageTypes.SendPasswordResetCode
            };

            _dispatcher.SendEmail(request);

            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }

        [Test, Category("Integration")]
        public void ShouldSendPasswordResetConfirmationEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = TokenGenerator.CreatePasswordResetConfirmationTokens(),
                MessageType = MessageTypes.PasswordChanged
            };

            _dispatcher.SendEmail(request);

            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }

        [Test, Category("Integration")]
        public void ShouldSendDailyDigestEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = TokenGenerator.CreateVacanciesAboutToExpireTokens(4),
                MessageType = MessageTypes.DailyDigest
            };

            _dispatcher.SendEmail(request);

            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }
    }
}