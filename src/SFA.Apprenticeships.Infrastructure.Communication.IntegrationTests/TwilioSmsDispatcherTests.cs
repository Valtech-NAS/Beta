namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using Application.Interfaces.Communications;
    using Logging.IoC;
    using NUnit.Framework;
    using Domain.Entities.Exceptions;
    using Common.IoC;
    using IoC;
    using Sms;
    using StructureMap;

    [TestFixture]
    public class TwilioSmsDispatcherTests
    {
        private ISmsDispatcher _dispatcher;
        private ISmsDispatcher _voidSmsDispatcher;
        private const string InvalidTestToNumber = "+34615691671";
        private const string TestToNumber = "+447972527913";

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommunicationRegistry>();
            });

            _dispatcher = container.GetInstance<ISmsDispatcher>("TwilioSmsDispatcher");
            _voidSmsDispatcher = container.GetInstance<ISmsDispatcher>("VoidSmsDispatcher");
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShoudConstructTwillioEmailDispatcher()
        {
            Assert.IsNotNull(_dispatcher);
            Assert.IsInstanceOf<TwilioSmsDispatcher>(_dispatcher);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldConstructVoidSmsDispatcher()
        {
            Assert.IsNotNull(_voidSmsDispatcher);
            Assert.IsInstanceOf<VoidSmsDispatcher>(_voidSmsDispatcher);
        }

        [Test, Category("Integration"), Ignore("Ignoring until a solution can be found that doesn't send the sms")]
        public void ShoudSendSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateActivationEmailTokens(),
                MessageType = MessageTypes.SendActivationCode
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration"), Ignore("Ignoring until a solution can be found that doesn't send the sms")]
        public void ShoudSendSmsWithFromEmailInTemplateConfiguration()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateActivationEmailTokens(),
                MessageType = MessageTypes.SendActivationCode
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration"), Ignore("Ignoring until a solution can be found that doesn't send the sms")]
        public void ShouldSendAccountUnlockCode()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateAccountUnlockCodeTokens(),
                MessageType = MessageTypes.SendAccountUnlockCode
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration"), Ignore("Ignoring until a solution can be found that doesn't send the sms")]
        public void ShouldSendApprenticeshipApplicationSubmittedSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateApprenticeshipApplicationSubmittedTokens(),
                MessageType = MessageTypes.ApprenticeshipApplicationSubmitted
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration"), Ignore("Ignoring until a solution can be found that doesn't send the sms")]
        public void ShouldSendTraineeshipApplicationSubmittedSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateTraineeshipApplicationSubmittedTokens(),
                MessageType = MessageTypes.TraineeshipApplicationSubmitted
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration"), Ignore("Ignoring until a solution can be found that doesn't send the sms")]
        public void ShouldSendPasswordResetCodeSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreatePasswordResetTokens(),
                MessageType = MessageTypes.SendPasswordResetCode
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration"), Ignore("Ignoring until a solution can be found that doesn't send the sms")]
        public void ShouldSendPasswordResetConfirmationSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreatePasswordResetConfirmationTokens(),
                MessageType = MessageTypes.PasswordChanged
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration"), Ignore("Ignoring until a solution can be found that doesn't send the sms")]
        public void ShoudSendDailyDigestSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateVacanciesAboutToExpireTokens(2),
                MessageType = MessageTypes.DailyDigest
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration"), ExpectedException(typeof(CustomException))]
        public void ShouldGetExceptionIfSomethingHappens()
        {
            var request = new SmsRequest
            {
                ToNumber = InvalidTestToNumber,
                Tokens = TokenGenerator.CreatePasswordResetConfirmationTokens(),
                MessageType = MessageTypes.PasswordChanged
            };

            _dispatcher.SendSms(request);
        }
    }
}