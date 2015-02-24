namespace SFA.Apprenticeships.Infrastructure.Communication.IoC
{
    using System.Collections.Generic;
    using Application.Interfaces.Communications;
    using Email;
    using Email.EmailMessageFormatters;
    using Sms;
    using Sms.SmsMessageFormatters;
    using StructureMap.Configuration.DSL;

    public class CommunicationRegistry : Registry
    {
        public CommunicationRegistry()
        {
            IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>> emailMessageFormatters = new[]
            {
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendActivationCode, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendAccountUnlockCode, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendPasswordResetCode, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.PasswordChanged, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.ApprenticeshipApplicationSubmitted, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.TraineeshipApplicationSubmitted, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.DailyDigest, new EmailDailyDigestMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.CandidateContactMessage, new EmailSimpleMessageFormatter()),
            };
            For<IEmailDispatcher>().Use<SendGridEmailDispatcher>().Named("SendGridEmailDispatcher")
                .Ctor<IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>>>().Is(emailMessageFormatters);

            For<IEmailDispatcher>().Use<VoidEmailDispatcher>().Name = "VoidEmailDispatcher";
            For<ISmsDispatcher>().Use<VoidSmsDispatcher>().Name = "VoidSmsDispatcher";
            For<SendGridConfiguration>().Singleton().Use(SendGridConfiguration.Instance);
            For<ITwillioConfiguration>().Singleton().Use(TwilioConfiguration.Instance);

            IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>> smsMessageFormatters = new[]
            {
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.SendAccountUnlockCode, new SmsAccountUnlockCodeMessageFormatter(TwilioConfiguration.Instance)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.SendPasswordResetCode, new SmsPasswordResetCodeMessageFormatter(TwilioConfiguration.Instance)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.PasswordChanged, new SmsPasswordChangedMessageFormatter(TwilioConfiguration.Instance)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationSubmitted, new SmsApprenticeshipApplicationSubmittedMessageFormatter(TwilioConfiguration.Instance)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.TraineeshipApplicationSubmitted, new SmsTraineeshipApplicationSubmittedMessageFormatter(TwilioConfiguration.Instance)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.DailyDigest, new SmsDailyDigestMessageFormatter(TwilioConfiguration.Instance)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.SendMobileVerificationCode, new SmsSendMobileVerificationCodeFormatter(TwilioConfiguration.Instance))
            };

            For<ISmsDispatcher>().Use<TwilioSmsDispatcher>().Named("TwilioSmsDispatcher")
                .Ctor<IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>>>().Is(smsMessageFormatters);
        }
    }
}