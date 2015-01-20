namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using Application.Interfaces.Messaging;
    using Common.IoC;
    using Email;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class SendGridEmailDispatcherTests
    {
        private IEmailDispatcher _dispatcher;

        private IEmailDispatcher _voidEmailDispatcher;

        private const string TestToEmail = "vincentredrum@gmail.com";


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
        }

        [Test, Category("Integration")]
        public void ShouldSendVacanciesAboutToExpireEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = TokenGenerator.CreateVacanciesAboutToExpireTokens(),
                MessageType = MessageTypes.DailyDigest
            };

            _dispatcher.SendEmail(request);
        }
    }
}