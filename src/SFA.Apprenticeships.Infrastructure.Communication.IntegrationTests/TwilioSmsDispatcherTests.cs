﻿namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;
    using SFA.Apprenticeships.Domain.Entities.Exceptions;
    using SFA.Apprenticeships.Infrastructure.Common.IoC;
    using SFA.Apprenticeships.Infrastructure.Communication.IoC;
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
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommunicationRegistry>();
                x.AddRegistry<CommonRegistry>();
            });

            _dispatcher = ObjectFactory.GetNamedInstance<ISmsDispatcher>("TwilioSmsDispatcher");
            _voidSmsDispatcher = ObjectFactory.GetNamedInstance<ISmsDispatcher>("VoidSmsDispatcher");
#pragma warning restore 0618
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

        [Test, Category("Integration")]
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

        [Test, Category("Integration"), Category("SmokeTests")]
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

        [Test, Category("Integration")]
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

        [Test, Category("Integration")]
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

        [Test, Category("Integration")]
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

        [Test, Category("Integration")]
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

        [Test, Category("Integration")]
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