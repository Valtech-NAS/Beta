namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System.Collections.Generic;

    public interface ITwillioConfiguration
    {
        string AccountSid { get; }

        string AuthToken { get; }

        string MobileNumberFrom { get; }

        IEnumerable<TwilioTemplateConfiguration> Templates { get; }
    }
}