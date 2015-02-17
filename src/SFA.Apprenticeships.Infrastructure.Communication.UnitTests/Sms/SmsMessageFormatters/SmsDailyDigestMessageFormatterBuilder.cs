namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using Communication.Sms;
    using Communication.Sms.SmsMessageFormatters;
    using Moq;

    public class SmsDailyDigestMessageFormatterBuilder
    {
        private Mock<ITwillioConfiguration> _twilioConfiguration;

        public SmsDailyDigestMessageFormatterBuilder()
        {
            _twilioConfiguration = new Mock<ITwillioConfiguration>();
        }

        public SmsDailyDigestMessageFormatterBuilder With(Mock<ITwillioConfiguration> twilioConfiguration)
        {
            _twilioConfiguration = twilioConfiguration;
            return this;
        }

        public SmsDailyDigestMessageFormatterBuilder WithMessageTemplate(string messageTemplate)
        {
            _twilioConfiguration = new Mock<ITwillioConfiguration>();
            var templates = new List<TwilioTemplateConfiguration>
            {
                new TwilioTemplateConfiguration
                {
                    Name = SmsDailyDigestMessageFormatter.TemplateName,
                    Message = messageTemplate
                }
            };
            _twilioConfiguration.Setup(tc => tc.Templates).Returns(templates);
            return this;
        }

        public SmsDailyDigestMessageFormatter Build()
        {
            var formatter = new SmsDailyDigestMessageFormatter(_twilioConfiguration.Object);
            return formatter;
        }
    }
}