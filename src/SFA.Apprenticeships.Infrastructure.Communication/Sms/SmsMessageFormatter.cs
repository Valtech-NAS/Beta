namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Application.Interfaces.Communications;

    public abstract class SmsMessageFormatter
    {
        private readonly ITwillioConfiguration _configuration;
        protected string Message;

        protected SmsMessageFormatter(ITwillioConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected TwilioTemplateConfiguration GetTemplateConfiguration(string templateName)
        {
            var template = _configuration.Templates.FirstOrDefault(each => each.Name == templateName);

            if (template != null)
            {
                return template;
            }

            var errorMessage = string.Format("GetTemplateConfiguration : Invalid SMS template name: {0}", templateName);

            throw new ConfigurationErrorsException(errorMessage);
        }

        public abstract string GetMessage(IEnumerable<CommunicationToken> communicationTokens);
    }
}
