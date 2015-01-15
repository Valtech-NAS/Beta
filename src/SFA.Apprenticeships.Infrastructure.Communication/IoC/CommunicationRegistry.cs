namespace SFA.Apprenticeships.Infrastructure.Communication.IoC
{
    using System.Collections.Generic;
    using Application.Interfaces.Messaging;
    using Configuration;
    using StructureMap.Configuration.DSL;

    public class CommunicationRegistry : Registry
    {
        public CommunicationRegistry()
        {
            For<IEmailDispatcher>().Use<SendGridEmailDispatcher>().Name = "SendGridEmailDispatcher";
            For<IEmailDispatcher>().Use<VoidEmailDispatcher>().Name = "VoidEmailDispatcher";
            For<ISmsDispatcher>().Use<VoidSmsDispatcher>().Name = "VoidSmsDispatcher";
            For<SendGridConfiguration>().Singleton().Use(SendGridConfiguration.Instance);
            For<TwilioConfiguration>().Singleton().Use(TwilioConfiguration.Instance);

            IEnumerable<KeyValuePair<MessageTypes, SmsMessageGenerator>> messageGenerators = new[]
            {
                new KeyValuePair<MessageTypes, SmsMessageGenerator>(MessageTypes.SendActivationCode, new SmsActivationCodeMessageGenerator(TwilioConfiguration.Instance) ),
                new KeyValuePair<MessageTypes, SmsMessageGenerator>(MessageTypes.SendAccountUnlockCode, new SmsAccountUnlockCodeMessageGenerator(TwilioConfiguration.Instance)),
                new KeyValuePair<MessageTypes, SmsMessageGenerator>(MessageTypes.SendPasswordResetCode, new SmsPasswordResetCodeMessageGenerator(TwilioConfiguration.Instance)),
                new KeyValuePair<MessageTypes, SmsMessageGenerator>(MessageTypes.PasswordChanged, new SmsPasswordChangedMessageGenerator(TwilioConfiguration.Instance)),
                new KeyValuePair<MessageTypes, SmsMessageGenerator>(MessageTypes.ApprenticeshipApplicationSubmitted, new SmsApprenticeshipApplicationSubmittedMessageGenerator(TwilioConfiguration.Instance)),
                new KeyValuePair<MessageTypes, SmsMessageGenerator>(MessageTypes.TraineeshipApplicationSubmitted, new SmsTraineeshipApplicationSubmittedMessageGenerator(TwilioConfiguration.Instance))
            };

            For<ISmsDispatcher>().Use<TwilioSmsDispatcher>().Named("TwilioSmsDispatcher")
                .Ctor<IEnumerable<KeyValuePair<MessageTypes, SmsMessageGenerator>>>().Is(messageGenerators);
        }
    }
}